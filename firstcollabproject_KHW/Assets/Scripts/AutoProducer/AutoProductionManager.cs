using System;
using UnityEngine;

public class AutoProductionManager : MonoBehaviour
{
    public static AutoProductionManager Instance;

    [Header("자동 생산 설정")]
    [Tooltip("자동 생산 반영 주기")]
    [SerializeField] private float tickInterval = 1f;

    private long currentProductionPerSecond;
    private float timer;

    public long CurrentProductionPerSecond => currentProductionPerSecond;

    public event Action<long> OnAutoProductionChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        OnAutoProductionChanged?.Invoke(currentProductionPerSecond);
    }

    private void Update()
    {
        if (ResourceManager.Instance == null)
            return;

        timer += Time.deltaTime;

        if (timer >= tickInterval)
        {
            timer -= tickInterval;

            long amount = Mathf.RoundToInt(currentProductionPerSecond * tickInterval);
            if (amount > 0)
                ResourceManager.Instance.AddResource(amount);
        }
    }

    public void SetProductionPerSecond(long value)
    {
        currentProductionPerSecond = (long)Mathf.Max(0, value);
        OnAutoProductionChanged?.Invoke(currentProductionPerSecond);
    }
}