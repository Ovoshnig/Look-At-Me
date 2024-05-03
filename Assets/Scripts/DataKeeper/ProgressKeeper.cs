using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ProgressKeeper
{
    private int _achievedLevel;
    private int _currentLevel;
    private const string _achievedLevelKey = "AchievedLevel";

    private ProgressKeeper()
    {
        _achievedLevel = PlayerPrefs.GetInt(_achievedLevelKey, 1);
        _currentLevel = 0;
    }

    public int GetAchievedLevel() => _achievedLevel;

    public void SetAchievedLevel(int level)
    {
        if (level > 0 && level < SceneManager.sceneCountInBuildSettings)
        {
            _achievedLevel = level;
            PlayerPrefs.SetInt(_achievedLevelKey, level);
        }
        else
        {
            throw new InvalidOperationException("Invalid level");
        }
    }

    public void UpdateCurrentLevel() => _currentLevel = SceneManager.GetActiveScene().buildIndex;

    public int GetCurrentLevel() => _currentLevel;

    public void SetCurrentLevel(int level)
    {
        if (level >= 0 && level <= _achievedLevel)
        {
            _currentLevel = level;
        }
        else
        {
            throw new InvalidOperationException("Invalid level");
        }
    }
}
