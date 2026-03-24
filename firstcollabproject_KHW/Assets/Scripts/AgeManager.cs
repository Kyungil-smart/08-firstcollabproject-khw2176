using UnityEngine;

public enum Age
{
    Ancient, Medieval, EarlyModern, Modern, Space
}

public class AgeManager : MonoBehaviour
{
    public Age currentAge;
    public int level;

    public void LevelUp()
    {
        level++;

        if (level >= 10) currentAge = Age.Medieval;
        if (level >= 20) currentAge = Age.EarlyModern;
        if (level >= 30) currentAge = Age.Modern;
        if (level >= 40) currentAge = Age.Space;
    }
}