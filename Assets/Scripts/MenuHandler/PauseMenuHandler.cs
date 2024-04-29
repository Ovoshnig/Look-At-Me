using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PauseMenuHandler : MenuHandler
{
    private FPSController _fpsController;
    private bool _isGamePaused;

    protected override void InitializeSettings()
    {
        _sensitivity = PlayerPrefs.GetFloat(SensitivityKey);
        _sensitivitySlider.value = _sensitivity;

        _volume = PlayerPrefs.GetFloat(VolumeKey);
        _volumeSlider.value = _volume;

        _optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _fpsController = FindObjectOfType<FPSController>();
        _fpsController.RotationSpeed = _sensitivity;

        ProgressSaver.UpdateCurrentLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseOrResume(!_isGamePaused);
    }

    public void PauseOrResume(bool shouldBePaused)
    {
        _isGamePaused = shouldBePaused;

        Cursor.lockState = _isGamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isGamePaused;

        _optionsPanel.SetActive(_isGamePaused);

        _fpsController.IsCanMove = !_isGamePaused;

        if (!shouldBePaused)
            _fpsController.RotationSpeed = _sensitivity;
    }

    public void ResetLevel()
    {
        SaveSensitivityAndVolume();

        int currentLevel = ProgressSaver.GetCurrentLevel();
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadPreviousLevel()
    {
        SaveSensitivityAndVolume();

        int currentLevel = ProgressSaver.GetCurrentLevel();

        if (currentLevel > 1)
            SceneManager.LoadScene(currentLevel - 1);
    }

    public void LoadNextLevel(bool isLevelComplete)
    {
        SaveSensitivityAndVolume();

        int achievedLevel = ProgressSaver.GetAchievedLevel();
        int currentLevel = ProgressSaver.GetCurrentLevel();

        if (isLevelComplete && currentLevel + 1 > achievedLevel)
        {
            currentLevel++;
            ProgressSaver.SetCurrentLevel(currentLevel);
            if (achievedLevel < SceneManager.sceneCountInBuildSettings - 2)
            {
                achievedLevel++;
                ProgressSaver.SetAchievedLevel(achievedLevel);
            }

            SceneManager.LoadScene(currentLevel);
        }
        else if (currentLevel < achievedLevel)
        {
            currentLevel++;

            SceneManager.LoadScene(currentLevel);
        }
    }

    public void LoadMenu()
    {
        SaveSensitivityAndVolume();

        SceneManager.LoadScene(0);
    }
}
