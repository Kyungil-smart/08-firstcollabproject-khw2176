using UnityEngine;

public enum UpgradeCategory
{
    Building,
    Worker
}

[CreateAssetMenu(fileName = "UpgradeData", menuName = "MiningGame/Upgrade Data")]
public class UpgradeDataSO : ScriptableObject
{
    [Header("기본 정보")]
    [Tooltip("중복되지 않는 고유 ID")]
    public string upgradeId;

    [Tooltip("UI에 표시될 이름")]
    public string displayName;

    [Tooltip("설명")]
    [TextArea(2, 5)]
    public string description;

    [Tooltip("아이콘")]
    public Sprite icon;

    [Header("분류")]
    [Tooltip("해당 시대")]
    public EraType eraType;

    [Tooltip("건물 / 인부")]
    public UpgradeCategory category;

    [Header("해금 조건")]
    [Tooltip("이 레벨 이상부터 구매 가능")]
    public int unlockLevel = 1;

    [Header("구매 설정")]
    [Tooltip("기본 구매 비용")]
    public long baseCost = 10;

    [Tooltip("구매 비용 증가 배율")]
    public float costMultiplier = 1.15f;

    [Tooltip("최대 보유 가능 수")]
    public int maxOwnedCount = 99;

    [Header("강화 설정")]
    [Tooltip("최대 강화 레벨")]
    public int maxUpgradeLevel = 10;

    [Tooltip("기본 강화 비용")]
    public long upgradeBaseCost = 30;

    [Tooltip("강화 비용 증가 배율")]
    public float upgradeCostMultiplier = 1.2f;

    [Header("생산량 설정")]
    [Tooltip("1개당 기본 초당 생산량")]
    public long baseProductionPerSecond = 1;

    [Tooltip("강화 1레벨당 증가 생산량")]
    public long productionPerUpgradeLevel = 1;

    [Header("월드 오브젝트")]
    [Tooltip("구매 시 배치할 월드 프리팹")]
    public GameObject worldPrefab;

    [Tooltip("배치 위치")]
    public Vector3 spawnPosition;

    [Tooltip("배치 회전")]
    public Vector3 spawnRotation;
}