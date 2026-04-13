using System;
using UnityEngine;

public class ClickPowerManager : MonoBehaviour
{
    public static ClickPowerManager Instance;

    [Header("도구 클릭 파워")]
    [Tooltip("기본 클릭 파워")]
    [SerializeField] private int baseClickPower = 1;

    [Tooltip("도구 레벨 1당 증가량")]
    [SerializeField] private int clickPowerPerToolLevel = 1;

    [Tooltip("현재 도구 레벨")]
    [SerializeField] private int currentToolLevel = 1;

    public int CurrentToolLevel => currentToolLevel;
    public int CurrentClickPower => baseClickPower + ((currentToolLevel - 1) * clickPowerPerToolLevel);

    public event Action<int> OnClickPowerChanged;

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
        OnClickPowerChanged?.Invoke(CurrentClickPower);
    }

    public void SetToolLevel(int level)
    {
        currentToolLevel = Mathf.Max(1, level);
        OnClickPowerChanged?.Invoke(CurrentClickPower);
    }
}