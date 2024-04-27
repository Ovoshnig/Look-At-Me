using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProgressSaver
{
    private const string _achievedLevelKey = "AchievedLevel";

    private static int s_AchievedLevel;
    private static int s_CurrentLevel;

    static ProgressSaver()
    {
        s_AchievedLevel = PlayerPrefs.GetInt(_achievedLevelKey, 1);
        s_CurrentLevel = 0;
    }

    public static int GetAchievedLevel() => s_AchievedLevel;

    public static void SetAchievedLevel(int level)
    {
        if (level > 0 && level < SceneManager.sceneCountInBuildSettings)
        {
            s_AchievedLevel = level; 
            PlayerPrefs.SetInt(_achievedLevelKey, level);
        }
    }

    public static void UpdateCurrentLevel() => s_CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    
    public static int GetCurrentLevel() => s_CurrentLevel;

    public static void SetCurrentLevel(int level) 
    {
        if (level >= 0 && level <= s_AchievedLevel) 
            s_CurrentLevel = level;
    }
}
