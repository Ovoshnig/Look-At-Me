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
    private readonly GameSettingsInstaller.LevelSettings _levelSettings;
    private uint _achievedLevel;
    private uint _currentLevel;
    private bool _isLevelLoading = false;

    public event Action LevelLoading;
    public event Action LevelLoaded;

    [Inject]
    public LevelSwitch(DataSaver dataKeeper, GameSettingsInstaller.LevelSettings levelSettings)
    {
        _dataKeeper = dataKeeper;
        _levelSettings = levelSettings;
        _achievedLevel = _dataKeeper.LoadData(AchievedLevelKey, _levelSettings.FirstGameplayLevel);
        _currentLevel = (uint)SceneManager.GetActiveScene().buildIndex;

        if (_currentLevel > _achievedLevel && _currentLevel <= _levelSettings.LastGameplayLevel)
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

    public void ResetProgress() => _achievedLevel = _levelSettings.FirstGameplayLevel;

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
            if (_achievedLevel < _levelSettings.LastGameplayLevel)
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
        if (_currentLevel > _levelSettings.FirstGameplayLevel)
            await LoadLevel(_currentLevel - 1);
    }

    public void LoadCurrentLevel() => LoadLevel(_currentLevel).Forget();

    public async UniTask LoadLevel(uint value)
    {
        if (value < 0 || value > _levelSettings.LastGameplayLevel + 1)
            throw new InvalidOperationException("Invalid level");

        LevelLoading?.Invoke();
        _isLevelLoading = true;

        if (value >= _levelSettings.FirstGameplayLevel && value <= _levelSettings.LastGameplayLevel)
        {
            await SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
            await UniTask.WaitForSeconds(_levelSettings.LevelTransitionDuration / 2);
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            await SceneManager.LoadSceneAsync((int)value, LoadSceneMode.Additive);

            await UniTask.WaitForSeconds(_levelSettings.LevelTransitionDuration / 2);
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
