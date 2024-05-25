using System;
using Zenject;

public class LookSettings : IDisposable
{
    private const string SensitivityKey = "Sensitivity";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.GameSettings _settings;
    private float _sensitivity;

    [Inject]
    public LookSettings(DataSaver dataSaver, GameSettingsInstaller.GameSettings settings)
    {
        _dataSaver = dataSaver;
        _settings = settings;
        _sensitivity = _dataSaver.LoadData(SensitivityKey, _settings.MaxSensitivity * _settings.DefaultSliderCoefficient);
        Sensitivity = _sensitivity;
    }

    public float Sensitivity
    {
        get
        {
            return _sensitivity;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxSensitivity)
                _sensitivity = value;
        }
    }

    public void Dispose() => _dataSaver.SaveData(SensitivityKey, _sensitivity);
}
