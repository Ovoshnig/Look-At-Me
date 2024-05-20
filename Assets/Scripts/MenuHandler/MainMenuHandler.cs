using UnityEngine;

public sealed class MainMenuHandler : MenuHandler
{
    [SerializeField] private GameObject _settingsPanel;

    protected override void InitializeSettings()
    {
        _settingsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continue() => LevelSwitch.LoadAchievedLevel();

    public void StartNewGame() => LevelSwitch.LoadFirstLevel();

    public void OpenSettings() => _settingsPanel.SetActive(true);

    public void Resume() => _settingsPanel.SetActive(false);

    public void ResetProgress() => LevelSwitch.ResetProgress();

    public void QuitGame() => Application.Quit();
}
