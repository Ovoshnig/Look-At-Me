using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameState : IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly LevelSwitch _levelSwitch;
    private bool _isGamePaused = false;
    private bool _reversePauseStateAllowed = true;

    public event Action GamePaused;
    public event Action GameUnpaused;

    [Inject]
    public GameState(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
        _levelSwitch.LevelLoading += OnLevelLoading;
        _levelSwitch.LevelLoaded += OnLevelLoaded;

        _playerInput = new PlayerInput();
        _playerInput.GameState.ReversePauseState.performed += ReversePauseState;
        _playerInput.Enable();

        SetPauseState(pause: false);
    }

    public void Dispose()
    {
        _levelSwitch.LevelLoading -= OnLevelLoading;
        _levelSwitch.LevelLoaded -= OnLevelLoaded;
        _playerInput.Disable();
    }

    public void Pause()
    {
        SetPauseState(pause: true);
        GamePaused?.Invoke();
    }

    public void Unpause()
    {
        SetPauseState(pause: false);
        GameUnpaused?.Invoke();
    }

    private void OnLevelLoading()
    {
        Unpause();
        _reversePauseStateAllowed = false;
    }

    private void OnLevelLoaded() => _reversePauseStateAllowed = true;

    private void SetPauseState(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        _isGamePaused = pause;
    }

    private void ReversePauseState(InputAction.CallbackContext context)
    {
        if (_reversePauseStateAllowed)
        {
            if (_isGamePaused)
                Unpause();
            else
                Pause();
        }
    }
}
