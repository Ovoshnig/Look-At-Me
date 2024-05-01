using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource),
                  typeof(VideoPlayer))]
public sealed class VideoPlayerSwitch : SelectableObject
{
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private AudioSource _audioSource;

    private void OnValidate()
    {
        if (_videoPlayer == null || _audioSource == null)
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            _audioSource = GetComponent<AudioSource>();

            _audioSource.playOnAwake = false;
            _videoPlayer.playOnAwake = false;
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.SetTargetAudioSource(0, _audioSource);
        }
    }
    
    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
            _videoPlayer.Play();
        else
            _videoPlayer.Stop();
    }
}
