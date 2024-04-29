using UnityEngine;
using UnityEngine.SceneManagement;

public static class ProgressSaver
{
    private static int s_achievedLevel;
    private static int s_currentLevel;
    private const string _achievedLevelKey = "AchievedLevel";

    static ProgressSaver()
    {
        s_achievedLevel = PlayerPrefs.GetInt(_achievedLevelKey, 1);
        s_currentLevel = 0;
    }

    public static int GetAchievedLevel() => s_achievedLevel;

    public static void SetAchievedLevel(int level)
    {
        if (level > 0 && level < SceneManager.sceneCountInBuildSettings)
        {
            s_achievedLevel = level; 
            PlayerPrefs.SetInt(_achievedLevelKey, level);
        }
    }

    public static void UpdateCurrentLevel() => s_currentLevel = SceneManager.GetActiveScene().buildIndex;
    
    public static int GetCurrentLevel() => s_currentLevel;

    public static void SetCurrentLevel(int level) 
    {
        if (level >= 0 && level <= s_achievedLevel) 
            s_currentLevel = level;
    }
}
