using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenuHandler : MenuHandler
{
    protected override void InitializeSettings()
    {
        _sensitivity = PlayerPrefs.GetFloat(SensitivityKey, _sensitivitySlider.maxValue / 2);
        _sensitivitySlider.value = _sensitivity;

        _volume = PlayerPrefs.GetFloat(VolumeKey, _volumeSlider.maxValue / 2);
        _volumeSlider.value = _volume;

        _optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadAchievedLevel()
    {
        SaveSensitivityAndVolume();

        int achievedLevel = ProgressSaver.GetAchievedLevel();
        SceneManager.LoadScene(achievedLevel);
    }

    public void LoadFirstLevel()
    {
        SaveSensitivityAndVolume();

        int achievedLevel = 1;
        ProgressSaver.SetAchievedLevel(achievedLevel);
        SceneManager.LoadScene(achievedLevel);
    }

    public void OpenSettings()
    {
        _optionsPanel.SetActive(true);
    }

    public void Resume()
    {
        SaveSensitivityAndVolume();

        _optionsPanel.SetActive(false);
    }

    public void ResetProgress()
    {
        int achievedLevel = 1;

        ProgressSaver.SetAchievedLevel(achievedLevel);
    }

    public void QuitGame()
    {
        SaveSensitivityAndVolume();
        Application.Quit();
    }
}
