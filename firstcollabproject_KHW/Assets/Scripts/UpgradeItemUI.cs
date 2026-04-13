using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    [Header("연결된 업그레이드 데이터")]
    [Tooltip("이 UI가 표시할 업그레이드 데이터")]
    [SerializeField] private UpgradeDataSO data;

    [Header("UI - 기본 정보")]
    [Tooltip("아이콘 이미지")]
    [SerializeField] private Image iconImage;

    [Tooltip("이름 텍스트")]
    [SerializeField] private TextMeshProUGUI nameText;

    [Tooltip("설명 텍스트")]
    [SerializeField] private TextMeshProUGUI descText;

    [Header("UI - 상태 정보")]
    [Tooltip("보유 개수 표시")]
    [SerializeField] private TextMeshProUGUI ownedText;

    [Tooltip("강화 레벨 표시")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Tooltip("초당 생산량 표시")]
    [SerializeField] private TextMeshProUGUI productionText;

    [Header("UI - 비용 정보")]
    [Tooltip("구매 비용")]
    [SerializeField] private TextMeshProUGUI buyCostText;

    [Tooltip("강화 비용")]
    [SerializeField] private TextMeshProUGUI upgradeCostText;

    [Tooltip("해금 조건 텍스트")]
    [SerializeField] private TextMeshProUGUI conditionText;

    [Header("버튼")]
    [Tooltip("구매 버튼")]
    [SerializeField] private Button buyButton;

    [Tooltip("판매 버튼")]
    [SerializeField] private Button sellButton;

    [Tooltip("강화 버튼")]
    [SerializeField] private Button upgradeButton;

    private void Start()
    {
        if (buyButton != null) buyButton.onClick.AddListener(OnClickBuy);
        if (sellButton != null) sellButton.onClick.AddListener(OnClickSell);
        if (upgradeButton != null) upgradeButton.onClick.AddListener(OnClickUpgrade);

        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeStateChanged += RefreshUI;

        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged += OnResourceChanged;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelChanged += OnLevelChanged;
            LevelManager.Instance.OnEraChanged += OnEraChanged;
        }

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.OnUpgradeStateChanged -= RefreshUI;

        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= OnResourceChanged;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelChanged -= OnLevelChanged;
            LevelManager.Instance.OnEraChanged -= OnEraChanged;
        }
    }

    private void OnResourceChanged(long amount) => RefreshUI();
    private void OnLevelChanged(int level) => RefreshUI();
    private void OnEraChanged(EraType era) => RefreshUI();

    private void OnClickBuy()
    {
        UpgradeManager.Instance?.Buy(data);
    }

    private void OnClickSell()
    {
        UpgradeManager.Instance?.Sell(data);
    }

    private void OnClickUpgrade()
    {
        UpgradeManager.Instance?.Upgrade(data);
    }

    public void RefreshUI()
    {
        if (data == null || UpgradeManager.Instance == null)
            return;

        UpgradeRuntimeData runtime = UpgradeManager.Instance.GetRuntimeData(data);
        if (runtime == null)
            return;

        bool unlocked = UpgradeManager.Instance.IsUnlocked(data);
        long buyCost = UpgradeManager.Instance.GetBuyCost(data);
        long upCost = UpgradeManager.Instance.GetUpgradeCost(data);
        long production = UpgradeManager.Instance.GetProductionPerSecond(data);

        if (iconImage != null)
            iconImage.sprite = data.icon;

        if (nameText != null)
            nameText.text = data.displayName;

        if (descText != null)
            descText.text = data.description;

        if (ownedText != null)
            ownedText.text = $"보유 수 : {runtime.ownedCount}/{data.maxOwnedCount}";

        if (levelText != null)
            levelText.text = $"강화 Lv : {runtime.upgradeLevel}/{data.maxUpgradeLevel}";

        if (productionText != null)
            productionText.text = $"총 생산량 : {production:N0}/s";

        if (buyCostText != null)
            buyCostText.text = $"구매 비용 : {buyCost:N0}";

        if (upgradeCostText != null)
            upgradeCostText.text = $"강화 비용 : {upCost:N0}";

        if (conditionText != null)
        {
            conditionText.text = unlocked
                ? $"해금 완료 (Lv.{data.unlockLevel})"
                : $"Lv.{data.unlockLevel} 달성 시 해금";
        }

        if (buyButton != null)
            buyButton.interactable = UpgradeManager.Instance.CanBuy(data);

        if (sellButton != null)
            sellButton.interactable = UpgradeManager.Instance.CanSell(data);

        if (upgradeButton != null)
            upgradeButton.interactable = UpgradeManager.Instance.CanUpgrade(data);

        if (iconImage != null)
            iconImage.color = unlocked ? Color.white : Color.gray;
    }

    public UpgradeDataSO GetData()
    {
        return data;
    }
}