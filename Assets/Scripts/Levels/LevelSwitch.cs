using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelSwitch : IDisposable
{
    private const string AchievedLevelKey = "AchievedLevel";
    private const string TransitionSceneName = "TransitionScene";

    private readonly DataSaver _dataKeeper;
    private readonly GameSettingsInstaller.GameSettings _settings;
    private uint _achievedLevel;
    private uint _currentLevel;
    private bool _isLevelLoading = false;

    public event Action LevelLoading;
    public event Action LevelLoaded;

    [Inject]
    public LevelSwitch(DataSaver dataKeeper, GameSettingsInstaller.GameSettings settings)
    {
        _dataKeeper = dataKeeper;
        _settings = settings;
        _achievedLevel = _dataKeeper.LoadData(AchievedLevelKey, _settings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _settings.LastGameplayLevel)
            _achievedLevel = _currentLevel;

        WaitForFirstSceneLoad().Forget();
    }

    public void Dispose() => _dataKeeper.SaveData(AchievedLevelKey, _achievedLevel);

    public async UniTask LoadAchievedLevel() => await LoadLevel(_achievedLevel);

    public async UniTask LoadFirstLevel()
    {
        ResetProgress();
        await LoadAchievedLevel();
    }

    public void ResetProgress() => _achievedLevel = _settings.FirstGameplayLevel;

    public async UniTask LoadNextLevel()
    {
        if (_currentLevel < _achievedLevel)
            await LoadLevel(_currentLevel + 1);
    }

    public async UniTask<bool> TryLoadNextLevelFirstTime()
    {
        if (_isLevelLoading)
            return false;

        bool isAchievedNextLevel = _currentLevel == _achievedLevel;

        if (isAchievedNextLevel)
        {
            if (_achievedLevel < _settings.LastGameplayLevel)
                _achievedLevel++;

            await LoadLevel(_currentLevel + 1);
        }
        else
        {
            LoadNextLevel().Forget();
        }

        return isAchievedNextLevel;
    }

    public async UniTask LoadPreviousLevel()
    {
        if (_currentLevel > _settings.FirstGameplayLevel)
            await LoadLevel(_currentLevel - 1);
    }

    public void LoadCurrentLevel() => LoadLevel(_currentLevel).Forget();

    public async UniTask LoadLevel(uint value)
    {
        if (value < 0 || value > _settings.LastGameplayLevel + 1)
            throw new InvalidOperationException("Invalid level");

        LevelLoading?.Invoke();
        _isLevelLoading = true;

        if (value >= _settings.FirstGameplayLevel && value <= _settings.LastGameplayLevel)
        {
            await SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
            await UniTask.WaitForSeconds(_settings.LevelTransitionDuration / 2);
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            await SceneManager.LoadSceneAsync((int)value, LoadSceneMode.Additive);

            await UniTask.WaitForSeconds(_settings.LevelTransitionDuration / 2);
            await SceneManager.UnloadSceneAsync(TransitionSceneName);
        }
        else
        {
            await SceneManager.LoadSceneAsync((int)value);
        }

        LevelLoaded?.Invoke();
        _isLevelLoading = false;
        _currentLevel = value;
    }

    private async UniTaskVoid WaitForFirstSceneLoad()
    {
        await UniTask.WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        LevelLoaded?.Invoke();
    }
}
