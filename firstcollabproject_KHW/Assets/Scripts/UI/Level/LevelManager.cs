using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("현재 레벨 정보")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int requiredExp = 10;

    [Header("경험치 1 구매당 자원 소모")]
    [SerializeField] private int expBuyCost = 5;

    private EraType currentEra = EraType.Ancient;

    public int CurrentLevel => currentLevel;
    public int CurrentExp => currentExp;
    public int RequiredExp => requiredExp;
    public int ExpBuyCost => expBuyCost;
    public EraType CurrentEra => currentEra;

    public event Action<EraType> OnEraChanged;
    public event Action<int> OnLevelChanged;
    public event Action OnExpChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentEra = GetEraByLevel(currentLevel);
        requiredExp = GetRequiredExp(currentLevel);
    }

    private void Start()
    {
        OnLevelChanged?.Invoke(currentLevel);
        OnExpChanged?.Invoke();
        OnEraChanged?.Invoke(currentEra);
    }

    public void TryBuyExp()
    {
        if (ResourceManager.Instance == null)
            return;

        if (!ResourceManager.Instance.TrySpendResource(expBuyCost))
        {
            Debug.Log("자원이 부족합니다.");
            return;
        }

        AddExp(1);
    }

    private void AddExp(int amount)
    {
        currentExp += amount;

        while (currentExp >= requiredExp)
        {
            currentExp -= requiredExp;
            LevelUp();
        }

        OnExpChanged?.Invoke();
    }

    private void LevelUp()
    {
        currentLevel++;
        requiredExp = GetRequiredExp(currentLevel);
        expBuyCost += 5;

        Debug.Log($"레벨 업! 현재 레벨: {currentLevel}");

        OnLevelChanged?.Invoke(currentLevel);

        EraType newEra = GetEraByLevel(currentLevel);
        if (newEra != currentEra)
        {
            currentEra = newEra;
            Debug.Log($"시대 변경! 현재 시대: {currentEra}");
            OnEraChanged?.Invoke(currentEra);
        }
    }

    private int GetRequiredExp(int level)
    {
        if (level <= 10)
            return 10 + (level - 1) * 5;   // 10, 15, 20 ...

        if (level <= 20)
            return 60 + (level - 11) * 10; // 60, 70, 80 ...

        if (level <= 30)
            return 160 + (level - 21) * 15; // 160, 175, 190 ...

        return 310 + (level - 31) * 20;    // 310, 330, 350 ...
    }

    public EraType GetEraByLevel(int level)
    {
        if (level <= 10) return EraType.Ancient;
        if (level <= 20) return EraType.Medieval;
        if (level <= 30) return EraType.Modern;
        return EraType.Space;
    }

    public string GetLevelRangeText()
    {
        return currentEra switch
        {
            EraType.Ancient => "고대 시대 (Lv.1 ~ 10)",
            EraType.Medieval => "중세 시대 (Lv.11 ~ 20)",
            EraType.Modern => "근/현대 시대 (Lv.21 ~ 30)",
            EraType.Space => "우주 시대 (Lv.31 ~ 40)",
            _ => ""
        };
    }
}