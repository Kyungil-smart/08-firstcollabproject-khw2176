using TMPro;
using UnityEngine;

public class RightLevelPanelUI : MonoBehaviour
{
    [Header("우측 레벨 패널 UI")]
    [Tooltip("현재 시대 텍스트")]
    [SerializeField] private TextMeshProUGUI currentEraText;

    [Tooltip("시대 레벨 범위 텍스트")]
    [SerializeField] private TextMeshProUGUI eraRangeText;

    private void Start()
    {
        RefreshUI();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelChanged += OnLevelChanged;
            LevelManager.Instance.OnEraChanged += OnEraChanged;
        }
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelChanged -= OnLevelChanged;
            LevelManager.Instance.OnEraChanged -= OnEraChanged;
        }
    }

    private void OnLevelChanged(int level)
    {
        RefreshUI();
    }

    private void OnEraChanged(EraType era)
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (LevelManager.Instance == null)
            return;

        EraType era = LevelManager.Instance.CurrentEra;

        currentEraText.text = $"현재 시대 : {GetEraName(era)}";
        eraRangeText.text = LevelManager.Instance.GetLevelRangeText();
    }

    private string GetEraName(EraType era)
    {
        return era switch
        {
            EraType.Ancient => "고대",
            EraType.Medieval => "중세",
            EraType.Modern => "근/현대",
            EraType.Space => "우주",
            _ => "알 수 없음"
        };
    }
}