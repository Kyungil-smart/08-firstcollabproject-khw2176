using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public double resource;
    public double dps;
    public double clickPower = 1;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        resource += dps * Time.deltaTime;
    }

    public bool Use(double amount)
    {
        if (resource < amount) return false;
        resource -= amount;
        return true;
    }
}