using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private const string MusicClipsPath = "Audio/Music";

    private AudioSource _musicSource;
    private System.Random _random;
    private List<AudioClip> _musicTracks;
    private Queue<AudioClip> _trackQueue;

    private void Awake()
    {
        if (_musicSource == null)
            _musicSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _random = new System.Random();
        LoadMusicTracks();
        ShuffleAndQueueTracks();
        PlayNextTrack().Forget();
    }

    private AudioClip GetNextTrack()
    {
        if (_trackQueue.Count == 0)
        {
            ShuffleAndQueueTracks();
        }

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
        var tracks = new List<AudioClip>(_musicTracks);
        _trackQueue = new Queue<AudioClip>();

        while (tracks.Count > 0)
        {
            int index = _random.Next(tracks.Count);
            _trackQueue.Enqueue(tracks[index]);
            tracks.RemoveAt(index);
        }
    }

    private async UniTaskVoid PlayNextTrack()
    {
        while (true)
        {
            var clip = GetNextTrack();
            _musicSource.clip = clip;
            _musicSource.Play();
            await UniTask.WaitForSeconds(clip.length);
        }
    }
}
