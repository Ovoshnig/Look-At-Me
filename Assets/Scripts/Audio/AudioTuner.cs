using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioTuner : IDisposable
{
    private const string SoundsVolumeKey = "SoundsVolume";
    private const string MusicVolumeKey = "MusicVolume";

    private readonly DataSaver _dataSaver;
    private readonly GameSettingsInstaller.Settings _settings;
    private readonly GameSettingsInstaller.AudioSettings _audioSettings;

    private readonly GameState _gameState;
    private readonly AudioMixerGroup _audioMixerGroup;
    private readonly MusicPlayer _musicPlayer;
    private float _soundsVolume;
    private float _musicVolume;

    [Inject]
    public AudioTuner(DataSaver dataSaver, GameSettingsInstaller.Settings settings, GameSettingsInstaller.AudioSettings audioSettings,
                         GameState gameState, AudioMixerGroup audioMixerGroup, MusicPlayer musicPlayer)
    {
        _dataSaver = dataSaver;
        _settings = settings;
        _audioSettings = audioSettings;
        _gameState = gameState;
        _audioMixerGroup = audioMixerGroup;
        _musicPlayer = musicPlayer;

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
            if (value >= _audioSettings.MinVolume && value <= _audioSettings.MaxVolume)
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
            if (value >= _audioSettings.MinVolume && value <= _audioSettings.MaxVolume)
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
        _soundsVolume = _dataSaver.LoadData(SoundsVolumeKey, _audioSettings.MaxVolume * _settings.DefaultCoefficient);
        _musicVolume = _dataSaver.LoadData(MusicVolumeKey, _audioSettings.MaxVolume * _settings.DefaultCoefficient);
        SoundsVolume = _soundsVolume;
        MusicVolume = _musicVolume;
    }

    private void SubscribeToEvents()
    {
        _gameState.GamePaused += PauseSoundSources;
        _gameState.GameUnpaused += UnpauseSoundSources;
        _musicPlayer.MusicTrackChanged += FadeInMusicVolume;
    }

    private void UnsubscribeFromEvents()
    {
        _gameState.GamePaused -= PauseSoundSources;
        _gameState.GameUnpaused -= UnpauseSoundSources;
        _musicPlayer.MusicTrackChanged -= FadeInMusicVolume;
    }

    private void SaveVolumeData()
    {
        _dataSaver.SaveData(SoundsVolumeKey, _soundsVolume);
        _dataSaver.SaveData(MusicVolumeKey, _musicVolume);
    }

    private void SetSoundSourcesPauseState(bool pause)
    {
        if (pause)
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _audioSettings.MinVolume);
        else
            _audioMixerGroup.audioMixer.SetFloat(SoundsVolumeKey, _soundsVolume);
    }

    private async void FadeInMusicVolume()
    {
        float duration = _audioSettings.MusicFadeInDuration;
        float elapsedTime = 0f;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;
            float volume = Mathf.Lerp(_audioSettings.MinVolume, MusicVolume, t);
            _audioMixerGroup.audioMixer.SetFloat(MusicVolumeKey, volume);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        _audioMixerGroup.audioMixer.SetFloat(MusicVolumeKey, MusicVolume);
    }
}
