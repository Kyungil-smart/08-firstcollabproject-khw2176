using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public double baseCost;
    public double multiplier;
    public double value;
    public bool isClick;
}