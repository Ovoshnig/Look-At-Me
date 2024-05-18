using System;
using UnityEngine;
using Zenject;

public class AudioSettings : IDisposable
{
    private const string GlobalVolumeKey = "GlobalVolume";

    private readonly Settings _settings;
    private readonly DataKeeper<float> _globalVolumeKeeper;

    [Inject]
    public AudioSettings(Settings settings)
    {
        _settings = settings;
        _globalVolumeKeeper = new DataKeeper<float>(GlobalVolumeKey, _settings.MaxVolume * _settings.DefaultSliderCoefficient);
        GlobalVolume = _globalVolumeKeeper.Value;
    }

    public float GlobalVolume
    {
        get
        {
            return _globalVolumeKeeper.Value;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
            {
                _globalVolumeKeeper.Value = value;
                AudioListener.volume = value;
            }
        }
    }

    public void Dispose() => _globalVolumeKeeper.Dispose();
}
