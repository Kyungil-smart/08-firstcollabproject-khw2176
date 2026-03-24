using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public void OnClickMine()
    {
        ResourceManager.Instance.resource +=
            ResourceManager.Instance.clickPower;
    }
}