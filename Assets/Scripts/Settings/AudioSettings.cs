using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class AudioSettings : IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.GameSettings _settings;
    private readonly GameState _gameState;
    private readonly AudioSource _musicSource;
    private readonly LevelSwitch _levelSwitch;
    private float _soundsVolume;
    private float _musicVolume;
    private List<AudioSource> _soundSources = new();

    [Inject]
    public AudioSettings(DataSaver dataSaver, GameSettingsInstaller.GameSettings settings, 
                         GameState gameState, LevelSwitch levelSwitch, AudioSource musicSource)
    {
        _dataSaver = dataSaver;
        _settings = settings;
        _gameState = gameState;
        _levelSwitch = levelSwitch;
        _musicSource = musicSource;

        InitializeVolumeData();
        SubscribeToEvents();
    }

    public float SoundsVolume
    {
        get
        {
            return _soundsVolume;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
                _soundsVolume = value;
        }
    }
    
    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            if (value >= 0 && value <= _settings.MaxVolume)
            {
                _musicVolume = value;
                _musicSource.volume = value;
            }
        }
    }

    public void Dispose()
    {
        SaveVolumeData();
        UnsubscribeFromEvents();
    }

    public void PauseSoundSources() => SetSoundSourcesPauseState(pause: true);

    public void UnpauseSoundSources() => SetSoundSourcesPauseState(pause: false);

    private void InitializeVolumeData()
    {
        _soundsVolume = _dataSaver.LoadData(SoundsVolumeKey, _settings.MaxVolume * _settings.DefaultSliderCoefficient);
        _musicVolume = _dataSaver.LoadData(MusicVolumeKey, _settings.MaxVolume * _settings.DefaultSliderCoefficient);
        SoundsVolume = _soundsVolume;
        MusicVolume = _musicVolume;
    }

    private void SubscribeToEvents()
    {
        _levelSwitch.LevelLoaded += GetAudioSources;
        _levelSwitch.LevelLoaded += SetSourcesVolume;
        _gameState.GamePaused += GetAudioSources;
        _gameState.GamePaused += PauseSoundSources;
        _gameState.GameUnpaused += SetSourcesVolume;
        _gameState.GameUnpaused += UnpauseSoundSources;
    }

    private void SaveVolumeData()
    {
        _dataSaver.SaveData(SoundsVolumeKey, _soundsVolume);
        _dataSaver.SaveData(MusicVolumeKey, _musicVolume);
    }

    private void UnsubscribeFromEvents()
    {
        _levelSwitch.LevelLoaded -= GetAudioSources;
        _levelSwitch.LevelLoaded -= SetSourcesVolume;
        _gameState.GamePaused -= GetAudioSources;
        _gameState.GamePaused -= PauseSoundSources;
        _gameState.GameUnpaused -= SetSourcesVolume;
        _gameState.GameUnpaused -= UnpauseSoundSources;
    }

    private void GetAudioSources()
    {
        _soundSources = UnityEngine.Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList();
        _soundSources.Remove(_musicSource);
    }

    private void SetSourcesVolume()
    {
        foreach (AudioSource soundSource in _soundSources)
            if (soundSource != null)
                soundSource.volume = _soundsVolume;
    }

    private void SetSoundSourcesPauseState(bool pause)
    {
        foreach (AudioSource soundSource in _soundSources)
        {
            if (soundSource != null)
            {
                if (pause)
                    soundSource.Pause();
                else
                    soundSource.UnPause();
            }
        }
    }
}
