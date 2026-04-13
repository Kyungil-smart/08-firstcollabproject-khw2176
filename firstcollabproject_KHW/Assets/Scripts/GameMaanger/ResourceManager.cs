using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [Header("자원 관리")]
    [Tooltip("현재 보유 자원")]
    [SerializeField] private long resource = 0;

    public long Resource => resource;

    public event Action<long> OnResourceChanged;

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
        OnResourceChanged?.Invoke(resource);
    }

    public bool HasEnough(long amount)
    {
        return resource >= amount;
    }

    public bool TrySpendResource(long amount)
    {
        if (amount < 0)
            return false;

        if (resource < amount)
            return false;

        resource -= amount;
        OnResourceChanged?.Invoke(resource);

        Debug.Log($"자원 사용: {amount}, 남은 자원: {resource}");
        return true;
    }

    public void AddResource(long amount)
    {
        resource += amount;

        if (resource < 0)
            resource = 0;

        OnResourceChanged?.Invoke(resource);
        Debug.Log($"자원 변화: {amount}, 총 자원: {resource}");
    }
}