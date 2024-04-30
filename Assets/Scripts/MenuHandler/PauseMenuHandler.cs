using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PauseMenuHandler : MenuHandler
{
    private FPSController _fpsController;
    private bool _isGamePaused;

    protected override void InitializeSettings()
    {
        _optionsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _fpsController = FindObjectOfType<FPSController>();
        _fpsController.RotationSpeed = SensitivityKeeper.Get();

        ProgressKeeper.UpdateCurrentLevel();
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
            _fpsController.RotationSpeed = SensitivityKeeper.Get();
    }

    public void ResetLevel()
    {
        SaveData();

        int currentLevel = ProgressKeeper.GetCurrentLevel();
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadPreviousLevel()
    {
        SaveData();

        int currentLevel = ProgressKeeper.GetCurrentLevel();

        if (currentLevel > 1)
            SceneManager.LoadScene(currentLevel - 1);
    }

    public void LoadNextLevel(bool isLevelComplete)
    {
        SaveData();

        int achievedLevel = ProgressKeeper.GetAchievedLevel();
        int currentLevel = ProgressKeeper.GetCurrentLevel();

        if (isLevelComplete && currentLevel + 1 > achievedLevel)
        {
            currentLevel++;
            ProgressKeeper.SetCurrentLevel(currentLevel);
            if (achievedLevel < SceneManager.sceneCountInBuildSettings - 2)
            {
                achievedLevel++;
                ProgressKeeper.SetAchievedLevel(achievedLevel);
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
        SaveData();

        SceneManager.LoadScene(0);
    }
}
