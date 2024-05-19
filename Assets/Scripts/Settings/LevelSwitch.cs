using System;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwitch : IDisposable
{
    private const string AchievedLevelKey = "AchievedLevel";

    private readonly Settings _settings;
    private readonly DataKeeper<uint> _achievedLevelKeeper;
    private uint _currentLevel;

    [Inject]
    public LevelSwitch(Settings settings)
    {
        _settings = settings;
        _achievedLevelKeeper = new DataKeeper<uint>(AchievedLevelKey, _settings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevelKeeper.Value && _achievedLevelKeeper.Value < SceneManager.sceneCountInBuildSettings - 2)
            _achievedLevelKeeper.Value = _currentLevel;
    }

    public void LoadAchievedLevel()
    {
        _currentLevel = _achievedLevelKeeper.Value;
        SceneManager.LoadScene((int)_currentLevel);
    }

    public void LoadFirstLevel()
    {
        ResetProgress();

        _currentLevel = 1;
        SceneManager.LoadScene((int)_currentLevel);
    }

    public void ResetProgress() => _achievedLevelKeeper.Value = _settings.FirstGameplayLevel;

    public void LoadNextLevel()
    {
        if (_currentLevel < _achievedLevelKeeper.Value)
        {
            _currentLevel++;
            SceneManager.LoadScene((int)_currentLevel);
        }
    }

    public bool TryLoadNextLevelFirstTime()
    {
        bool isAchievedNextLevel = _currentLevel == _achievedLevelKeeper.Value;

        if (isAchievedNextLevel)
        {
            _currentLevel++;

            if (_achievedLevelKeeper.Value < SceneManager.sceneCountInBuildSettings - 2)
                _achievedLevelKeeper.Value++;

            SceneManager.LoadScene((int)_currentLevel);
        }
        else
        {
            LoadNextLevel();
        }

        return isAchievedNextLevel;
    }

    public void LoadPreviousLevel()
    {
        if (_currentLevel > 1)
        {
            _currentLevel--;
            SceneManager.LoadScene((int)_currentLevel);
        }
    }

    public void LoadLevel(int value)
    {
        if (value <= _achievedLevelKeeper.Value)
        {
            _currentLevel = (uint)value;
            SceneManager.LoadScene(value);
        }
        else
        {
            throw new InvalidOperationException("Invalid level");
        }
    }

    public void LoadCurrentLevel() => SceneManager.LoadScene((int)_currentLevel);

    public void Dispose() => _achievedLevelKeeper.Dispose();
}
