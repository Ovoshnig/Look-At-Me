using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwitch : IDisposable
{
    private const string AchievedLevelKey = "AchievedLevel";

    private readonly GameSettingsInstaller.GameSettings _settings;
    private readonly DataKeeper<uint> _achievedLevelKeeper;
    private uint _currentLevel;

    public event Action LevelLoaded;

    [Inject]
    public LevelSwitch(GameSettingsInstaller.GameSettings settings)
    {
        _settings = settings;
        _achievedLevelKeeper = new DataKeeper<uint>(AchievedLevelKey, _settings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevelKeeper.Value && _achievedLevelKeeper.Value < SceneManager.sceneCountInBuildSettings - 2)
            _achievedLevelKeeper.Value = _currentLevel;

        WaitForFirstSceneLoad().Forget();
    }

    public void LoadAchievedLevel()
    {
        _currentLevel = _achievedLevelKeeper.Value;
        LoadLevel(_currentLevel).Forget();
    }

    public void LoadFirstLevel()
    {
        ResetProgress();

        _currentLevel = 1;
        LoadLevel(_currentLevel).Forget();
    }

    public void ResetProgress() => _achievedLevelKeeper.Value = _settings.FirstGameplayLevel;

    public void LoadNextLevel()
    {
        if (_currentLevel < _achievedLevelKeeper.Value)
        {
            _currentLevel++;
            LoadLevel(_currentLevel).Forget();
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

    public void Dispose() => _achievedLevelKeeper.Dispose();

    private async UniTaskVoid WaitForFirstSceneLoad()
    {
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        LevelLoaded?.Invoke();
    }
}
