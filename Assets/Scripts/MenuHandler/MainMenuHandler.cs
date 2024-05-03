using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuHandler : MenuHandler
{
    protected override void InitializeSettings()
    {
        _optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadAchievedLevel()
    {
        SaveData();

        int achievedLevel = _progressKeeper.GetAchievedLevel();
        SceneManager.LoadScene(achievedLevel);
    }

    public void LoadFirstLevel()
    {
        SaveData();

        int achievedLevel = 1;
        _progressKeeper.SetAchievedLevel(achievedLevel);
        SceneManager.LoadScene(achievedLevel);
    }

    public void OpenSettings()
    {
        _optionsPanel.SetActive(true);
    }

    public void Resume()
    {
        SaveData();

        _optionsPanel.SetActive(false);
    }

    public void ResetProgress()
    {
        int achievedLevel = 1;
        _progressKeeper.SetAchievedLevel(achievedLevel);
    }

    public void QuitGame()
    {
        SaveData();
        Application.Quit();
    }
}
