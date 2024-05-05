using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenuHandler : MenuHandler
{
    [SerializeField] private Image _playerPoint;

    private FPSController _fpsController;
    private PlayerInput _playerInput;
    private bool _isGamePaused;

    [Inject]
    private void Construct(FPSController fPSController)
    {
        _fpsController = fPSController;

        _playerInput = new PlayerInput();
        _playerInput.PauseMenu.PauseOrResume.performed += PauseOrResume;
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    protected override void InitializeSettings()
    {
        _fpsController.RotationSpeed = _sensitivityKeeper.Value;

        Resume();
        _isGamePaused = false;
    }

    private void PauseOrResume(InputAction.CallbackContext context)
    {
        if (_isGamePaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        ChangePauseState(shouldBePaused: true);
        _isGamePaused = true;
    }

    public void Resume()
    {
        ChangePauseState(shouldBePaused: false);
        _isGamePaused = false;

        _fpsController.RotationSpeed = _sensitivityKeeper.Value;
    }

    private void ChangePauseState(bool shouldBePaused)
    {
        Cursor.lockState = shouldBePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldBePaused;

        _optionsPanel.SetActive(shouldBePaused);
        _playerPoint.enabled = !shouldBePaused;

        _fpsController.CanMove = !shouldBePaused;
    }

    public void ResetLevel() => _levelSwitch.LoadCurrentLevel();

    public void LoadPreviousLevel() => _levelSwitch.LoadPreviousLevel();

    public void LoadNextLevel() => _levelSwitch.LoadNextLevel();

    public void LoadMainMenu() => _levelSwitch.LoadLevel(0);
}
