using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameState : IDisposable
{
    private readonly PlayerInput _playerInput;
    private readonly LevelSwitch _levelSwitch;
    private bool _isGamePaused = false;

    public event Action GamePaused;
    public event Action GameUnpaused;

    [Inject]
    public GameState(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
        _levelSwitch.LevelLoaded += Unpause;

        _playerInput = new PlayerInput();
        _playerInput.GameState.ReversePauseState.performed += ReversePauseState;
        _playerInput.Enable();

        SetPauseState(pause: false);
    }

    public void Dispose()
    {
        _levelSwitch.LevelLoaded -= Unpause;

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

    private void SetPauseState(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        _isGamePaused = pause;
    }

    private void ReversePauseState(InputAction.CallbackContext context)
    {
        if (_isGamePaused)
            Unpause();
        else
            Pause();
    }
}
