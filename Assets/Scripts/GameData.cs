using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static int levelUnlocked;

    public static void SaveLevel(int level)
    {
        if (LoadLevel() > level) return;
        PlayerPrefs.SetInt("Level", level);
        levelUnlocked = level;
    }

    public static int LoadLevel() {
        if (!PlayerPrefs.HasKey("Level")) 
        {
            levelUnlocked = 1;
            return 1;
        }
        else {
            levelUnlocked = PlayerPrefs.GetInt("Level");
            return levelUnlocked;

        }
    }

    public static void ClearLevel()
    {
        PlayerPrefs.SetInt("Level", 1);
        levelUnlocked = 1;
    }
}
