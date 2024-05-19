using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenuHandler : MenuHandler
{
    [SerializeField] private Image _playerPoint;

    private FPSController _FPSController;
    private PlayerInput _playerInput;
    private bool _isGamePaused;

    [Inject]
    private void Construct(FPSController FPSController)
    {
        _FPSController = FPSController;

        _playerInput = new PlayerInput();
        _playerInput.PauseMenu.PauseOrResume.performed += PauseOrResume;
    }

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    protected override void InitializeSettings()
    {
        _FPSController.RotationSpeed = LookSettings.Sensitivity;

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

        _FPSController.RotationSpeed = LookSettings.Sensitivity;
    }

    private void ChangePauseState(bool shouldBePaused)
    {
        Cursor.lockState = shouldBePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldBePaused;

        _optionsPanel.SetActive(shouldBePaused);
        _playerPoint.enabled = !shouldBePaused;

        Time.timeScale = shouldBePaused ? 0 : 1;
        _FPSController.CanMove = !shouldBePaused;

        if (shouldBePaused)
            AudioSettings.PauseSounds();
        else
            AudioSettings.UnPauseSounds();
    }

    public void ResetLevel() => LevelSwitch.LoadCurrentLevel();

    public void LoadPreviousLevel() => LevelSwitch.LoadPreviousLevel();

    public void LoadNextLevel() => LevelSwitch.LoadNextLevel();

    public void LoadMainMenu() => LevelSwitch.LoadLevel(0).Forget();
}
