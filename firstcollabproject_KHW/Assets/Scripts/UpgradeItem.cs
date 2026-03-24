using UnityEngine;
using TMPro;

public class UpgradeItem : MonoBehaviour
{
    public UpgradeData data;
    public int count;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    void Start()
    {
        UpdateUI();
    }

    double GetCost()
    {
        return data.baseCost * Mathf.Pow((float)data.multiplier, count);
    }

    public void Buy()
    {
        double cost = GetCost();

        if (ResourceManager.Instance.Use(cost))
        {
            count++;

            if (data.isClick)
                ResourceManager.Instance.clickPower += data.value;
            else
                ResourceManager.Instance.dps += data.value;

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        nameText.text = data.upgradeName;
        costText.text = GetCost().ToString("F0");
    }
}