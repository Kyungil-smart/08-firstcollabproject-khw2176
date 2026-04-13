using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public enum EraType
{
    Ancient,    // 1 ~ 10
    Medieval,   // 11 ~ 20
    Modern,     // 21 ~ 30
    Space       // 31 ~ 40
}

public class EraManager : MonoBehaviour
{
    [Header("광물 생성 위치")]
    [SerializeField] private Transform mineralSpawnPoint;

    [Header("시대별 중앙 광물 프리팹")]
    [SerializeField] private GameObject ancientMineralPrefab;
    [SerializeField] private GameObject medievalMineralPrefab;
    [SerializeField] private GameObject modernMineralPrefab;
    [SerializeField] private GameObject spaceMineralPrefab;

    [Header("시대별 배경 이미지")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite ancientBackground;
    [SerializeField] private Sprite medievalBackground;
    [SerializeField] private Sprite modernBackground;
    [SerializeField] private Sprite spaceBackground;

    private GameObject currentMineralInstance;

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnEraChanged += ChangeEra;
            ChangeEra(LevelManager.Instance.CurrentEra);
        }
    }

    private void OnDestroy()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnEraChanged -= ChangeEra;
        }
    }

    private void ChangeEra(EraType era)
    {
        ChangeMineral(era);
        ChangeBackground(era);

        Debug.Log($"시대 적용 완료: {era}");
    }

    private void ChangeMineral(EraType era)
    {
        if (currentMineralInstance != null)
        {
            Destroy(currentMineralInstance);
        }

        GameObject prefab = GetMineralPrefabByEra(era);

        if (prefab != null && mineralSpawnPoint != null)
        {
            currentMineralInstance = Instantiate(
                prefab,
                mineralSpawnPoint.position,
                mineralSpawnPoint.rotation
            );
        }
        else
        {
            Debug.LogWarning("광물 프리팹 또는 SpawnPoint가 연결되지 않았습니다.");
        }

        MineralClickHandler clickHandler = currentMineralInstance.GetComponent<MineralClickHandler>();
        if (clickHandler != null)
        {
            clickHandler.SetBaseAmount(GetBaseResourceByEra(era));
        }
    }

    private GameObject GetMineralPrefabByEra(EraType era)
    {
        return era switch
        {
            EraType.Ancient => ancientMineralPrefab,
            EraType.Medieval => medievalMineralPrefab,
            EraType.Modern => modernMineralPrefab,
            EraType.Space => spaceMineralPrefab,
            _ => null
        };
    }

    private void ChangeBackground(EraType era)
    {
        if (backgroundImage == null)
        {
            Debug.LogWarning("Background Image가 연결되지 않았습니다.");
            return;
        }

        backgroundImage.sprite = era switch
        {
            EraType.Ancient => ancientBackground,
            EraType.Medieval => medievalBackground,
            EraType.Modern => modernBackground,
            EraType.Space => spaceBackground,
            _ => backgroundImage.sprite
        };
    }

    private int GetBaseResourceByEra(EraType era)
    {
        return era switch
        {
            EraType.Ancient => 1,
            EraType.Medieval => 5,
            EraType.Modern => 10,
            EraType.Space => 15,
            _ => 1
        };
    }
}