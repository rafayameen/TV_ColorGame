using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    //settings prefs
    // Sound Settings
    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt("Sound", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetSound()
    {
        return PlayerPrefs.GetInt("Sound", 1) == 1; // Default: On
    }

    // Music Settings
    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt("Music", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetMusic()
    {
        return PlayerPrefs.GetInt("Music", 1) == 1; // Default: On
    }

    // Vibration Settings
    public static void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Vibration", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetVibration()
    {
        return PlayerPrefs.GetInt("Vibration", 1) == 1; // Default: On
    }

    // Sensitivity Settings
    public static void SetSensitivity(float value)
    {
        PlayerPrefs.SetFloat("Sensitivity", value);
        PlayerPrefs.Save();
    }

    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 1.5f);
    }

    public static void DestroyAllChildren(RectTransform content)
    {
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    // Game Mode Settings
    private const string GameModeKey = "GameMode";

    public static void SetGameMode(GameMode mode)
    {
        PlayerPrefs.SetInt(GameModeKey, (int)mode);
        PlayerPrefs.Save();
    }

    public static GameMode GetGameMode()
    {
        int defaultMode = (int)GameMode.backyard;
        int modeIndex = PlayerPrefs.GetInt(GameModeKey, defaultMode);

        if (System.Enum.IsDefined(typeof(GameMode), modeIndex))
        {
            return (GameMode)modeIndex;
        }

        return GameMode.backyard;
    }


    public static string GetSceneNameFromMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.backyard:
                return GameConstants.gameplayScene_Mode1;
            case GameMode.castle:
                return GameConstants.gameplayScene_Mode2;
            case GameMode.island:
                return GameConstants.gameplayScene_Mode3;
            default:
                return GameConstants.gameplayScene_Mode1;
        }
    }

    public static void SetUpgradeLevel(UpgradeType type, int level)
    {
        string key = $"UpgradeType_{type}";
        PlayerPrefs.SetInt(key, level);
        PlayerPrefs.Save();
    }

    public static bool IsTutorialComplete()
    {
        return  PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
        
    }

    public static int GetUpgradeLevel(UpgradeType type)
    {
        string key = $"UpgradeType_{type}";
        return PlayerPrefs.GetInt(key, 0);
    }


    public static bool AreTreasureOresCollected()
    {
        return PlayerPrefs.GetInt($"TreasureOresCollected_{GetGameMode()}", 0) == 1;

    }
    public static void SetTreasureOresCollected(bool collected= true)
    {
        PlayerPrefs.SetInt($"TreasureOresCollected_{GetGameMode()}", collected ? 1: 0);
        PlayerPrefs.Save();
    }

    public  static int CompareVersion(string v1, string v2)
    {
        string[] arr1 = v1.Split('.');
        string[] arr2 = v2.Split('.');
        int target = arr1.Length > arr2.Length ? arr2.Length : arr1.Length;
        for (int i = 0; i < target; i++)
        {
            bool a = int.TryParse(arr1[i], out int rs1);
            bool b = int.TryParse(arr2[i], out int rs2);
            if (!a || !b) return -1;
            if (rs1 == rs2) continue;
            return rs1 > rs2 ? 1 : -1;
        }
        return 0;
    }
}
