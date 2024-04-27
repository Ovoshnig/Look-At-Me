using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerSwitch : SelectableObject
{
    private VideoPlayer _videoPlayer;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;

        if (_isSelect)
            _videoPlayer.Play();
        else
            _videoPlayer.Stop();
    }
}
