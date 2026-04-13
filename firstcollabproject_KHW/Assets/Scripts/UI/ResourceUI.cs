using TMPro;
using UnityEngine;

public class ResourceUI : MonoBehaviour
{
    [Header("자원 UI")]
    [Tooltip("현재 자원 텍스트")]
    [SerializeField] private TextMeshProUGUI resourceText;

    [Tooltip("클릭 파워 텍스트")]
    [SerializeField] private TextMeshProUGUI clickPowerText;

    [Tooltip("자동 생산량 텍스트")]
    [SerializeField] private TextMeshProUGUI autoProductionText;

    private void Start()
    {
        RefreshAll();

        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged += OnResourceChanged;

        if (ClickPowerManager.Instance != null)
            ClickPowerManager.Instance.OnClickPowerChanged += OnClickPowerChanged;

        if (AutoProductionManager.Instance != null)
            AutoProductionManager.Instance.OnAutoProductionChanged += OnAutoProductionChanged;
    }

    private void OnDestroy()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;

        if (ClickPowerManager.Instance != null)
            ClickPowerManager.Instance.OnClickPowerChanged -= OnClickPowerChanged;

        if (AutoProductionManager.Instance != null)
            AutoProductionManager.Instance.OnAutoProductionChanged -= OnAutoProductionChanged;
    }

    private void OnResourceChanged(long value)
    {
        if (resourceText != null)
            resourceText.text = $"현재 자원 : {value:N0}";
    }

    private void OnClickPowerChanged(int value)
    {
        if (clickPowerText != null)
            clickPowerText.text = $"클릭 파워 : {value}";
    }

    private void OnAutoProductionChanged(long value)
    {
        if (autoProductionText != null)
            autoProductionText.text = $"초당 생산량 : {value:N0}/s";
    }

    private void RefreshAll()
    {
        if (ResourceManager.Instance != null)
            OnResourceChanged(ResourceManager.Instance.Resource);

        if (ClickPowerManager.Instance != null)
            OnClickPowerChanged(ClickPowerManager.Instance.CurrentClickPower);

        if (AutoProductionManager.Instance != null)
            OnAutoProductionChanged(AutoProductionManager.Instance.CurrentProductionPerSecond);
    }
}