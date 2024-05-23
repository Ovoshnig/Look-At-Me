using UnityEngine;
using UnityEngine.Video;
using Zenject;

[RequireComponent(typeof(AudioSource),
                  typeof(VideoPlayer))]
public sealed class VideoPlayerSwitch : SelectableObject
{
    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;
    private GameState _gameState;

    [Inject]
    private void Construct(GameState gameState) => _gameState = gameState;

    protected override void React()
    {
        if (IsSelected)
            _videoPlayer.Play();
        else
            _videoPlayer.Stop();
    }

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _videoPlayer.playOnAwake = false;
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        _videoPlayer.SetTargetAudioSource(0, _audioSource);
    }

    private void OnEnable()
    {
        _gameState.GamePaused += Pause;
        _gameState.GameUnpaused += Unpause;
    }

    private void OnDisable()
    {
        _gameState.GamePaused -= Pause;
        _gameState.GameUnpaused -= Unpause;
    }

    private void Pause()
    {
        if (IsSelected)
            _videoPlayer.Pause();
    }

    private void Unpause()
    {
        if (IsSelected)
            _videoPlayer.Play();
    }
}
