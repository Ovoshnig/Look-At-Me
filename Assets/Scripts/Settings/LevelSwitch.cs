using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwitch : IDisposable
{
    private const string AchievedLevelKey = "AchievedLevel";

    private readonly DataSaver _dataKeeper;
    private readonly GameSettingsInstaller.GameSettings _settings;
    private uint _achievedLevel;
    private uint _currentLevel;

    public event Action LevelLoaded;

    [Inject]
    public LevelSwitch(DataSaver dataKeeper, GameSettingsInstaller.GameSettings settings)
    {
        _dataKeeper = dataKeeper;
        _settings = settings;
        _achievedLevel = _dataKeeper.LoadData(AchievedLevelKey, _settings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _achievedLevel < SceneManager.sceneCountInBuildSettings - 2)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoad().Forget();
    }

    public void Dispose() => _dataKeeper.SaveData(AchievedLevelKey, _achievedLevel);

    public void LoadAchievedLevel()
    {
        _currentLevel = _achievedLevel;
        LoadLevel(_currentLevel).Forget();
    }

    public void LoadFirstLevel()
    {
        ResetProgress();

        _currentLevel = _settings.FirstGameplayLevel;
        LoadLevel(_currentLevel).Forget();
    }

    public void ResetProgress() => _achievedLevel = _settings.FirstGameplayLevel;

    public void LoadNextLevel()
    {
        if (_currentLevel < _achievedLevel)
        {
            _currentLevel++;
            LoadLevel(_currentLevel).Forget();
        }
    }

    public bool TryLoadNextLevelFirstTime()
    {
        bool isAchievedNextLevel = _currentLevel == _achievedLevel;

        if (isAchievedNextLevel)
        {
            _currentLevel++;

            if (_achievedLevel < SceneManager.sceneCountInBuildSettings - 2)
                _achievedLevel++;

            LoadLevel(_currentLevel).Forget();
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
            LoadLevel(_currentLevel).Forget();
        }
    }

    public void LoadCurrentLevel() => LoadLevel(_currentLevel).Forget();

    public async UniTaskVoid LoadLevel(uint value)
    {
        if (value < SceneManager.sceneCountInBuildSettings)
        {
            _currentLevel = value;
            await SceneManager.LoadSceneAsync((int)value);
            LevelLoaded?.Invoke();
        }
        else
        {
            throw new InvalidOperationException("Invalid level");
        }
    }

    private async UniTaskVoid WaitForFirstSceneLoad()
    {
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        LevelLoaded?.Invoke();
    }
}
