using System;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwitch
{
    private readonly AchievedLevelKeeper _achievedLevelKeeper;

    private int _currentLevel;

    [Inject]
    private LevelSwitch(AchievedLevelKeeper achievedLevelKeeper)
    {
        _achievedLevelKeeper = achievedLevelKeeper;

        _currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadAchievedLevel()
    {
        _currentLevel = _achievedLevelKeeper.Value;
        SceneManager.LoadScene(_currentLevel);
    }

    public void LoadFirstLevel()
    {
        ResetProgress();

        _currentLevel = 1;
        SceneManager.LoadScene(_currentLevel);
    }

    public void ResetProgress() => _achievedLevelKeeper.Value = 1;
    
    public void LoadNextLevel()
    {
        if (_currentLevel < _achievedLevelKeeper.Value)
        {
            _currentLevel++;
            SceneManager.LoadScene(_currentLevel);
        }
    }

    public void TryLoadNextLevelFirstTime()
    {
        if (_currentLevel == _achievedLevelKeeper.Value)
        {
            _currentLevel++;

            if (_achievedLevelKeeper.Value < SceneManager.sceneCountInBuildSettings - 2)
                _achievedLevelKeeper.Value++;

            SceneManager.LoadScene(_currentLevel);
        }
        else
        {
            LoadNextLevel();
        }
    }

    public void LoadPreviousLevel()
    {
        if (_currentLevel > 1)
        {
            _currentLevel--;
            SceneManager.LoadScene(_currentLevel);
        }
    }

    public void LoadLevel(int value)
    {
        if (value <= _achievedLevelKeeper.Value)
        {
            _currentLevel = value;
            SceneManager.LoadScene(value);
        }
        else
        {
            throw new InvalidOperationException("Invalid level");
        }
    }

    public void LoadCurrentLevel() => SceneManager.LoadScene(_currentLevel);
}
