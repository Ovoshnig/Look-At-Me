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
        _fpsController.RotationSpeed = _sensitivityKeeper.Get();

        _progressKeeper.UpdateCurrentLevel();
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
            _fpsController.RotationSpeed = _sensitivityKeeper.Get();
    }

    public void ResetLevel()
    {
        SaveData();

        int currentLevel = _progressKeeper.GetCurrentLevel();
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadPreviousLevel()
    {
        SaveData();

        int currentLevel = _progressKeeper.GetCurrentLevel();

        if (currentLevel > 1)
            SceneManager.LoadScene(currentLevel - 1);
    }

    public void LoadNextLevel(bool isLevelComplete)
    {
        SaveData();

        int achievedLevel = _progressKeeper.GetAchievedLevel();
        int currentLevel = _progressKeeper.GetCurrentLevel();

        if (isLevelComplete && currentLevel + 1 > achievedLevel)
        {
            currentLevel++;
            _progressKeeper.SetCurrentLevel(currentLevel);
            if (achievedLevel < SceneManager.sceneCountInBuildSettings - 2)
            {
                achievedLevel++;
                _progressKeeper.SetAchievedLevel(achievedLevel);
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
