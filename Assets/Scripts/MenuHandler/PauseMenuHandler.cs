using UnityEngine;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenuHandler : MenuHandler
{
    [SerializeField] private Image _playerPoint;

    private FPSController _fpsController;
    private bool _isGamePaused;

    [Inject]
    private void Construct(FPSController fPSController)
    {
        _fpsController = fPSController;
    }

    protected override void InitializeSettings()
    {
        _fpsController.RotationSpeed = _sensitivityKeeper.Value;

        Resume();
        _isGamePaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        ChangePauseState(true);
        _isGamePaused = true;
    }

    public void Resume()
    {
        ChangePauseState(false);
        _isGamePaused = false;

        _fpsController.RotationSpeed = _sensitivityKeeper.Value;
    }

    private void ChangePauseState(bool shouldBePaused)
    {
        Cursor.lockState = shouldBePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldBePaused;

        _optionsPanel.SetActive(shouldBePaused);
        _playerPoint.enabled = !shouldBePaused;

        _fpsController.IsCanMove = !shouldBePaused;
    }

    public void ResetLevel() => _levelSwitch.LoadCurrentLevel();

    public void LoadPreviousLevel() => _levelSwitch.LoadPreviousLevel();

    public void LoadNextLevel() => _levelSwitch.LoadNextLevel();

    public void LoadMainMenu() => _levelSwitch.LoadLevel(0);
}
