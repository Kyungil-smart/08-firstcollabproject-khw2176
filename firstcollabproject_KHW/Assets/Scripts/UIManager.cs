using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI resourceText;
    public TextMeshProUGUI dpsText;
    public TextMeshProUGUI clickText;

    void Update()
    {
        resourceText.text = ResourceManager.Instance.resource.ToString("F0");
        dpsText.text = "DPS: " + ResourceManager.Instance.dps.ToString("F1");
        clickText.text = "Click: " + ResourceManager.Instance.clickPower;
    }
}