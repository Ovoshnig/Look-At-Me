using Cysharp.Threading.Tasks;
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

    protected override void SubscribeToEvents()
    {
        base.SubscribeToEvents();

        _gameState.GamePaused += Pause;
        _gameState.GameUnpaused += Resume;
    }

    protected override void UnsubscribeFromEvents()
    {
        base.UnsubscribeFromEvents();

        _gameState.GamePaused -= Pause;
        _gameState.GameUnpaused -= Resume;
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _resetLevelButton.onClick.AddListener(ResetLevel);
        _loadNextLevelButton.onClick.AddListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.AddListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    protected override void RemoveButtonListeners()
    {
        base.RemoveButtonListeners();

        _resetLevelButton.onClick.RemoveListener(ResetLevel);
        _loadNextLevelButton.onClick.RemoveListener(LoadNextLevel);
        _loadPreviousLevelButton.onClick.RemoveListener(LoadPreviousLevel);
        _loadMainMenuButton.onClick.RemoveListener(LoadMainMenu);
    }

    [Inject]
    private void Construct(GameState gameState)
    {
        _gameState = gameState;
    }

    private void ResetLevel() => SceneSwitch.LoadCurrentLevel();

    private void LoadPreviousLevel() => SceneSwitch.LoadPreviousLevel().Forget();

    private void LoadNextLevel() => SceneSwitch.LoadNextLevel().Forget();

    private void LoadMainMenu() => SceneSwitch.LoadLevel(0).Forget();

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
