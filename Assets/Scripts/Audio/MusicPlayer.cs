using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private const string MusicClipsPath = "Audio/Music";

    private readonly CancellationTokenSource _cts = new();
    private AudioSource _musicSource;
    private System.Random _random;
    private List<AudioClip> _musicTracks;
    private Queue<AudioClip> _trackQueue;

    public event Action MusicTrackChanged;

    private void Awake() => _musicSource = GetComponent<AudioSource>();

    private void Start()
    {
        _random = new System.Random();
        LoadMusicTracks();
        ShuffleAndQueueTracks();
        PlayNextTrack().Forget();
    }

    private void OnDestroy()
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
        _musicTracks = new List<AudioClip>(Resources.LoadAll<AudioClip>(MusicClipsPath));

        if (_musicTracks.Count == 0)
            Debug.LogWarning($"No music tracks found in Resources/{MusicClipsPath}.");
    }

    private void ShuffleAndQueueTracks()
    {
        List<AudioClip> tracks = new(_musicTracks);
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
