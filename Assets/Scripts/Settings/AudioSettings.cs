using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AudioSettings : IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly GameSettingsInstaller.GameSettings _settings;
    private readonly AudioSource _musicSource;
    private readonly LevelSwitch _levelSwitch;
    private readonly DataKeeper<float> _soundsVolumeKeeper;
    private readonly DataKeeper<float> _musicVolumeKeeper;
    private List<AudioSource> _soundSources = new();

    [Inject]
    public AudioSettings(GameSettingsInstaller.GameSettings settings, LevelSwitch levelSwitch, AudioSource musicSource)
    {
        _settings = settings;
        _levelSwitch = levelSwitch;
        _musicSource = musicSource;

        _soundsVolumeKeeper = new DataKeeper<float>(SoundsVolumeKey, _settings.MaxVolume * _settings.DefaultSliderCoefficient);
        _musicVolumeKeeper = new DataKeeper<float>(MusicVolumeKey, _settings.MaxVolume * _settings.DefaultSliderCoefficient);
        SoundsVolume = _soundsVolumeKeeper.Value;
        MusicVolume = _musicVolumeKeeper.Value;

        _levelSwitch.LevelLoaded += GetAudioSources;
    }

    public float SoundsVolume
    {
        get
        {
            return _soundsVolumeKeeper.Value;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
            {
                _soundsVolumeKeeper.Value = value;
                SetSourcesVolume(value);
            }
        }
    }
    
    public float MusicVolume
    {
        get
        {
            return _musicVolumeKeeper.Value;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
            {
                _musicVolumeKeeper.Value = value;
                _musicSource.volume = value;
            }
        }
    }

    public void PauseSounds() => ChangeSoundsPauseState(shouldBePaused: true);

    public void UnPauseSounds() => ChangeSoundsPauseState(shouldBePaused: false);

    public void Dispose()
    {
        _soundsVolumeKeeper.Dispose();
        _musicVolumeKeeper.Dispose();

        _levelSwitch.LevelLoaded -= GetAudioSources;
    }

    private void SetSourcesVolume(float value)
    {
        foreach (AudioSource soundSource in _soundSources)
            if (soundSource != null)
                soundSource.volume = value;
    }

    private void GetAudioSources()
    {
        _soundSources = UnityEngine.Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList();
        _soundSources.Remove(_musicSource);

        SetSourcesVolume(_soundsVolumeKeeper.Value);
    }

    private void ChangeSoundsPauseState(bool shouldBePaused)
    {
        foreach (AudioSource soundSource in _soundSources)
        {
            if (soundSource != null)
            {
                if (shouldBePaused)
                    soundSource.Pause();
                else
                    soundSource.UnPause();
            }
        }
    }
}
