using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("전체 업그레이드 데이터")]
    [Tooltip("모든 건물/인부 데이터")]
    [SerializeField] private UpgradeDataSO[] allUpgradeData;

    [Header("월드 배치 부모")]
    [Tooltip("구매한 프리팹들을 넣을 부모")]
    [SerializeField] private Transform purchasedObjectsRoot;

    private readonly Dictionary<string, UpgradeRuntimeData> runtimeDataDict = new();
    private readonly Dictionary<string, GameObject> spawnedObjectDict = new();

    public event Action OnUpgradeStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeRuntimeData();
    }

    private void Start()
    {
        RecalculateTotalProduction();
        OnUpgradeStateChanged?.Invoke();
    }

    private void InitializeRuntimeData()
    {
        runtimeDataDict.Clear();

        foreach (UpgradeDataSO data in allUpgradeData)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.upgradeId))
                continue;

            if (runtimeDataDict.ContainsKey(data.upgradeId))
            {
                Debug.LogWarning($"중복된 upgradeId 발견: {data.upgradeId}");
                continue;
            }

            runtimeDataDict.Add(data.upgradeId, new UpgradeRuntimeData(data.upgradeId));
        }
    }

    public UpgradeDataSO[] GetAllUpgradeData()
    {
        return allUpgradeData;
    }

    public UpgradeRuntimeData GetRuntimeData(string upgradeId)
    {
        return runtimeDataDict.TryGetValue(upgradeId, out UpgradeRuntimeData data) ? data : null;
    }

    public UpgradeRuntimeData GetRuntimeData(UpgradeDataSO data)
    {
        if (data == null)
            return null;

        return GetRuntimeData(data.upgradeId);
    }

    public long GetBuyCost(UpgradeDataSO data)
    {
        UpgradeRuntimeData runtime = GetRuntimeData(data);
        if (runtime == null)
            return long.MaxValue;

        return Mathf.RoundToInt(data.baseCost * Mathf.Pow(data.costMultiplier, runtime.ownedCount));
    }

    public long GetUpgradeCost(UpgradeDataSO data)
    {
        UpgradeRuntimeData runtime = GetRuntimeData(data);
        if (runtime == null)
            return long.MaxValue;

        return Mathf.RoundToInt(data.upgradeBaseCost * Mathf.Pow(data.upgradeCostMultiplier, runtime.upgradeLevel));
    }

    public bool IsUnlocked(UpgradeDataSO data)
    {
        if (data == null || LevelManager.Instance == null)
            return false;

        return LevelManager.Instance.CurrentLevel >= data.unlockLevel;
    }

    public bool CanBuy(UpgradeDataSO data)
    {
        if (data == null || ResourceManager.Instance == null)
            return false;

        UpgradeRuntimeData runtime = GetRuntimeData(data);
        if (runtime == null)
            return false;

        if (!IsUnlocked(data))
            return false;

        if (runtime.ownedCount >= data.maxOwnedCount)
            return false;

        return ResourceManager.Instance.HasEnough(GetBuyCost(data));
    }

    public bool CanSell(UpgradeDataSO data)
    {
        UpgradeRuntimeData runtime = GetRuntimeData(data);
        return runtime != null && runtime.ownedCount > 0;
    }

    public bool CanUpgrade(UpgradeDataSO data)
    {
        if (data == null || ResourceManager.Instance == null)
            return false;

        UpgradeRuntimeData runtime = GetRuntimeData(data);
        if (runtime == null)
            return false;

        if (!IsUnlocked(data))
            return false;

        if (runtime.ownedCount <= 0)
            return false;

        if (runtime.upgradeLevel >= data.maxUpgradeLevel)
            return false;

        return ResourceManager.Instance.HasEnough(GetUpgradeCost(data));
    }

    public void Buy(UpgradeDataSO data)
    {
        if (!CanBuy(data))
            return;

        long cost = GetBuyCost(data);
        if (!ResourceManager.Instance.TrySpendResource(cost))
            return;

        UpgradeRuntimeData runtime = GetRuntimeData(data);
        runtime.ownedCount++;

        SpawnWorldObjectIfNeeded(data, runtime);

        RecalculateTotalProduction();
        OnUpgradeStateChanged?.Invoke();
    }

    public void Sell(UpgradeDataSO data)
    {
        if (!CanSell(data))
            return;

        UpgradeRuntimeData runtime = GetRuntimeData(data);
        runtime.ownedCount--;

        long refund = Mathf.RoundToInt(GetBuyCost(data) * 0.5f);
        ResourceManager.Instance.AddResource(refund);

        RecalculateTotalProduction();
        OnUpgradeStateChanged?.Invoke();
    }

    public void Upgrade(UpgradeDataSO data)
    {
        if (!CanUpgrade(data))
            return;

        long cost = GetUpgradeCost(data);
        if (!ResourceManager.Instance.TrySpendResource(cost))
            return;

        UpgradeRuntimeData runtime = GetRuntimeData(data);
        runtime.upgradeLevel++;

        RecalculateTotalProduction();
        OnUpgradeStateChanged?.Invoke();
    }

    public long GetProductionPerSecond(UpgradeDataSO data)
    {
        UpgradeRuntimeData runtime = GetRuntimeData(data);
        if (runtime == null)
            return 0;

        long oneUnitProduction = data.baseProductionPerSecond + (data.productionPerUpgradeLevel * runtime.upgradeLevel);
        return oneUnitProduction * runtime.ownedCount;
    }

    public long GetTotalProductionPerSecondByEra(EraType era)
    {
        long total = 0;

        foreach (UpgradeDataSO data in allUpgradeData)
        {
            if (data == null || data.eraType != era)
                continue;

            total += GetProductionPerSecond(data);
        }

        return total;
    }

    public void RecalculateTotalProduction()
    {
        long total = 0;

        foreach (UpgradeDataSO data in allUpgradeData)
        {
            if (data == null)
                continue;

            total += GetProductionPerSecond(data);
        }

        if (AutoProductionManager.Instance != null)
            AutoProductionManager.Instance.SetProductionPerSecond(total);
    }

    private void SpawnWorldObjectIfNeeded(UpgradeDataSO data, UpgradeRuntimeData runtime)
    {
        if (data.worldPrefab == null || runtime == null)
            return;

        if (runtime.spawnedInWorld)
            return;

        Quaternion rot = Quaternion.Euler(data.spawnRotation);
        Transform parent = purchasedObjectsRoot != null ? purchasedObjectsRoot : null;

        GameObject spawned = Instantiate(data.worldPrefab, data.spawnPosition, rot, parent);
        runtime.spawnedInWorld = true;
        spawnedObjectDict[data.upgradeId] = spawned;
    }
}