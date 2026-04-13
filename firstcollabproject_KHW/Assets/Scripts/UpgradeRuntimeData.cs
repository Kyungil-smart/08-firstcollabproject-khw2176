[System.Serializable]
public class UpgradeRuntimeData
{
    public string upgradeId;
    public int ownedCount;
    public int upgradeLevel;
    public bool spawnedInWorld;

    public UpgradeRuntimeData(string id)
    {
        upgradeId = id;
        ownedCount = 0;
        upgradeLevel = 0;
        spawnedInWorld = false;
    }
}