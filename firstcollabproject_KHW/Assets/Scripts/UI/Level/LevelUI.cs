using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [Header("레벨 UI")]
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI expInfoText;
    [SerializeField] private TextMeshProUGUI needResourceText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Button levelUpButton;

    private void Update()
    {
        if (LevelManager.Instance == null || ResourceManager.Instance == null)
            return;

        currentLevelText.text = $"Lv. {LevelManager.Instance.CurrentLevel}";
        expInfoText.text = $"경험치 : {LevelManager.Instance.CurrentExp} / {LevelManager.Instance.RequiredExp}";
        needResourceText.text = $"필요 자원 : {LevelManager.Instance.ExpBuyCost:0}";

        expSlider.maxValue = LevelManager.Instance.RequiredExp;
        expSlider.value = LevelManager.Instance.CurrentExp;

        levelUpButton.interactable =
            ResourceManager.Instance.Resource >= LevelManager.Instance.ExpBuyCost;
    }
}