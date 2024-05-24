using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenuHandler : MenuHandler
{
    [SerializeField] private Button _continueGameButton;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private Button _resetProgressButton;

    protected override void InitializeSettings()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    protected override void AddButtonListeners()
    {
        base.AddButtonListeners();

        _continueGameButton.onClick.AddListener(ContinueGame);
        _startNewGameButton.onClick.AddListener(StartNewGame);
        _quitGameButton.onClick.AddListener(QuitGame);
        _resetProgressButton.onClick.AddListener(ResetProgress);
    }

    private void ContinueGame() => LevelSwitch.LoadAchievedLevel();

    private void StartNewGame() => LevelSwitch.LoadFirstLevel();

    private void QuitGame() => Application.Quit();

    private void ResetProgress() => LevelSwitch.ResetProgress();
}
