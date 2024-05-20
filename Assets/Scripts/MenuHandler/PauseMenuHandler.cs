using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public sealed class PauseMenuHandler : MenuHandler
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Image _playerPoint;

    private FPSController _FPSController;
    private PlayerInput _playerInput;
    private bool _isGamePaused;

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

    public void ResetLevel() => LevelSwitch.LoadCurrentLevel();

    public void LoadPreviousLevel() => LevelSwitch.LoadPreviousLevel();

    public void LoadNextLevel() => LevelSwitch.LoadNextLevel();

    public void LoadMainMenu() => LevelSwitch.LoadLevel(0).Forget();

    public void OpenSettingsPanel()
    {
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _pausePanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    protected override void InitializeSettings()
    {
        _FPSController.RotationSpeed = LookSettings.Sensitivity;

        _playerInput.PauseMenu.PauseOrResume.performed += PauseOrResume;

        Resume();
        _settingsPanel.SetActive(false);
        _isGamePaused = false;
    }

    [Inject]
    private void Construct(FPSController FPSController) => _FPSController = FPSController;

    private void Awake() => _playerInput = new PlayerInput();

    private void OnEnable() => _playerInput.Enable();

    private void OnDisable() => _playerInput.Disable();

    private void PauseOrResume(InputAction.CallbackContext context)
    {
        if (_settingsPanel.activeSelf)
        {
            CloseSettingsPanel();
        }
        else
        {
            if (_isGamePaused)
                Resume();
            else
                Pause();
        }
    }

    private void ChangePauseState(bool shouldBePaused)
    {
        Cursor.lockState = shouldBePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldBePaused;

        _pausePanel.SetActive(shouldBePaused);
        _playerPoint.enabled = !shouldBePaused;

        Time.timeScale = shouldBePaused ? 0 : 1;
        _FPSController.CanMove = !shouldBePaused;

        if (shouldBePaused)
            AudioSettings.PauseSounds();
        else
            AudioSettings.UnPauseSounds();
    }
}
