using UnityEngine;

public sealed class MainMenuHandler : MenuHandler
{
    protected override void InitializeSettings()
    {
        _optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continue() => LevelSwitch.LoadAchievedLevel();

    public void StartNewGame() => LevelSwitch.LoadFirstLevel();

    public void OpenSettings() => _optionsPanel.SetActive(true);

    public void Resume() => _optionsPanel.SetActive(false);

    public void ResetProgress() => LevelSwitch.ResetProgress();

    public void QuitGame() => Application.Quit();
}
