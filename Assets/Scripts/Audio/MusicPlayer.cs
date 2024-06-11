using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class MusicPlayer : IDisposable
{
    private const string MusicClipsPath = "Audio/Music";
    private const string MainMenu = nameof(MainMenu);
    private const string GameLevels = nameof(GameLevels);
    private const string Credits = nameof(Credits);

    private readonly System.Random _random = new();
    private readonly SceneSwitch _sceneSwitch;
    private readonly AudioSource _musicSource;
    private List<AudioClip> _currentTracks;
    private List<AudioClip> _mainMenuTracks;
    private List<AudioClip> _gameLevelsTracks;
    private List<AudioClip> _creditsTracks;
    private Queue<AudioClip> _trackQueue;
    private CancellationTokenSource _cts = new();
    private bool _isGameLevelMusicPlaying = false;

    public event Action MusicTrackChanged;

    [Inject]
    public MusicPlayer([Inject(Id = "musicSource")] AudioSource musicSource, SceneSwitch sceneSwitch)
    {
        _musicSource = musicSource;
        _sceneSwitch = sceneSwitch;
        _sceneSwitch.SceneLoaded += OnLevelLoaded;

        LoadMusicTracks();
    }

    public void Dispose()
    {
        _sceneSwitch.SceneLoaded -= OnLevelLoaded;
        CancelToken();
    }

    private void OnLevelLoaded(SceneSwitch.Scene scene)
    {
        if (scene == SceneSwitch.Scene.MainMenu)
        {
            _currentTracks = _mainMenuTracks;
            _isGameLevelMusicPlaying = false;
        }
        else if (scene == SceneSwitch.Scene.GameLevel)
        {
            if (_isGameLevelMusicPlaying)
                return;

            _currentTracks = _gameLevelsTracks;
            _isGameLevelMusicPlaying = true;
        }
        else
        {
            _currentTracks = _creditsTracks;
            _isGameLevelMusicPlaying = false;
        }

        CancelToken();
        _cts = new CancellationTokenSource();

        if (_currentTracks.Count > 0)
        {
            ShuffleAndQueueTracks();
            PlayNextTrack().Forget();
        }
    } 

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }

    private AudioClip GetNextTrack()
    {
        if (_trackQueue.Count == 0)
            ShuffleAndQueueTracks();

        return _trackQueue.Dequeue();
    }

    private void LoadMusicTracks()
    {
        _mainMenuTracks = new List<AudioClip>(Resources.LoadAll<AudioClip>($"{MusicClipsPath}/{MainMenu}"));
        _gameLevelsTracks = new List<AudioClip>(Resources.LoadAll<AudioClip>($"{MusicClipsPath}/{GameLevels}"));
        _creditsTracks = new List<AudioClip>(Resources.LoadAll<AudioClip>($"{MusicClipsPath}/{Credits}"));

        if (_mainMenuTracks.Count == 0)
            Debug.LogWarning($"No music tracks found in Resources/{MusicClipsPath}/{MainMenu}.");
        if (_gameLevelsTracks.Count == 0)
            Debug.LogWarning($"No music tracks found in Resources/{MusicClipsPath}/{GameLevels}.");
        if (_creditsTracks.Count == 0)
            Debug.LogWarning($"No music tracks found in Resources/{MusicClipsPath}/{Credits}.");
    }

    private void ShuffleAndQueueTracks()
    {
        List<AudioClip> tracks = new(_currentTracks);
        _trackQueue = new Queue<AudioClip>();

        while (tracks.Count > 0)
        {
            int index = _random.Next(tracks.Count);
            _trackQueue.Enqueue(tracks[index]);
            tracks.RemoveAt(index);
        }
    }

    private async UniTask PlayNextTrack()
    {
        while (true)
        {
            AudioClip clip = GetNextTrack();
            _musicSource.clip = clip;
            _musicSource.Play();
            MusicTrackChanged?.Invoke();
            await UniTask.WaitWhile(() => _musicSource.isPlaying, cancellationToken: _cts.Token);
        }
    }
}
