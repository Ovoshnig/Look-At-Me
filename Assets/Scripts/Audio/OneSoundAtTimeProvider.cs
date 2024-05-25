using UnityEngine;

public class OneSoundAtTimeProvider
{
    private AudioSource _currentAudioSource;
    private AudioSource _previousAudioSource;

    public void SingleSound(AudioSource audioSource)
    {
        _previousAudioSource = _currentAudioSource;
        _currentAudioSource = audioSource;

        if (_previousAudioSource != null && _previousAudioSource != _currentAudioSource)
            _previousAudioSource.Stop();
    }
}
