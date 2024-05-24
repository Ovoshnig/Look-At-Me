using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenuHandler : MenuHandler
{
    [SerializeField] private GameObject _playerPoint;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _resetLevelButton;
    [SerializeField] private Button _loadNextLevelButton;
    [SerializeField] private Button _loadPreviousLevelButton;
    [SerializeField] private Button _loadMainMenuButton;

    private GameState _gameState;

    protected override void InitializeSettings() => Resume();

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _gameState.GamePaused += Pause;
        _gameState.GameUnpaused += Resume;

        _resumeButton.onClick.AddListener(_gameState.Unpause);
        _resetLevelButton.onClick.AddListener(ResetLevel);
        _loadNextLevelButton.onClick.AddListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.AddListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _gameState.GamePaused -= Pause;
        _gameState.GameUnpaused -= Resume;
    }

    [Inject]
    private void Construct(GameState gameState) => _gameState = gameState;

    private void ResetLevel() => LevelSwitch.LoadCurrentLevel();

    private void LoadPreviousLevel() => LevelSwitch.LoadPreviousLevel();

    private void LoadNextLevel() => LevelSwitch.LoadNextLevel();

    private void LoadMainMenu() => LevelSwitch.LoadLevel(0).Forget();

    private void Pause()
    {
        MenuPanel.SetActive(true);
        _playerPoint.SetActive(false);
    }

    private void Resume()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        _playerPoint.SetActive(true);
    }
}
