using System;
using UnityEngine.Audio;
using Zenject;

public class AudioSettings : IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.GameSettings _settings;
    private readonly GameState _gameState;
    private readonly AudioMixerGroup _audioMixerGroup;
    private float _soundsVolume;
    private float _musicVolume;

    [Inject]
    public AudioSettings(DataSaver dataSaver, GameSettingsInstaller.GameSettings settings, 
                         GameState gameState, AudioMixerGroup audioMixerGroup)
    {
        _dataSaver = dataSaver;
        _settings = settings;
        _gameState = gameState;
        _audioMixerGroup = audioMixerGroup;

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
            if (value >= _settings.MinVolume && value <= _settings.MaxVolume)
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
            if (value >= _settings.MinVolume && value <= _settings.MaxVolume)
            {
                _musicVolume = value;
                _audioMixerGroup.audioMixer.SetFloat(MusicVolumeKey, value);
            }
        }
    }

    public void Dispose()
    {
        UnsubscribeFromEvents();
        SaveVolumeData();
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
        _gameState.GamePaused += PauseSoundSources;
        _gameState.GameUnpaused += UnpauseSoundSources;
    }

    private void UnsubscribeFromEvents()
    {
        _gameState.GamePaused -= PauseSoundSources;
        _gameState.GameUnpaused -= UnpauseSoundSources;
    }

    private void SaveVolumeData()
    {
        _dataSaver.SaveData(SoundsVolumeKey, _soundsVolume);
        _dataSaver.SaveData(MusicVolumeKey, _musicVolume);
    }

    private void SetSoundSourcesPauseState(bool pause)
    {
        if (pause)
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _settings.MinVolume);
        else
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _soundsVolume);
    }
}
