using System;
using UnityEngine;
using Zenject;

public class LookSettings : IDisposable
{
    private const string SensitivityKey = "Sensitivity";

    private readonly GameSettingsInstaller.GameSettings _settings;
    private readonly DataKeeper<float> _sensitivityKeeper;

    [Inject]
    public LookSettings(GameSettingsInstaller.GameSettings settings)
    {
        _settings = settings;
        _sensitivityKeeper = new DataKeeper<float>(SensitivityKey, _settings.MaxSensitivity * _settings.DefaultSliderCoefficient);
        Sensitivity = _sensitivityKeeper.Value;
    }

    public float Sensitivity
    {
        get
        {
            return _sensitivityKeeper.Value;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxSensitivity)
                _sensitivityKeeper.Value = value;
        }
    }

    public void Dispose() => _sensitivityKeeper.Dispose();
}
