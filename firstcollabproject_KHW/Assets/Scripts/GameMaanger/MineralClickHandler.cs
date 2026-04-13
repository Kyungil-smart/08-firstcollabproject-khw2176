using UnityEngine;

public class MineralClickHandler : MonoBehaviour
{
    [Header("광물 설정")]
    [Tooltip("시대별 광물 기본 자원량")]
    [SerializeField] private int baseResourceAmount = 1;

    public void SetBaseAmount(int amount)
    {
        baseResourceAmount = amount;
    }

    public void OnClick()
    {
        if (ResourceManager.Instance == null)
            return;

        int clickPower = 0;
        if (ClickPowerManager.Instance != null)
            clickPower = ClickPowerManager.Instance.CurrentClickPower;

        long gain = baseResourceAmount + clickPower;
        ResourceManager.Instance.AddResource(gain);

        Debug.Log($"광물 클릭, 획득 자원: {gain}");
    }
}