using System.Collections.Generic;
using UnityEngine;

public class OneSoundAtTimeProvider : MonoBehaviour
{
    [SerializeField] private Transform _objectsTransform;
    [SerializeField] private List<AudioSource> _audioSources = new();

    private void Awake()
    {
        foreach (Transform childTransform in _objectsTransform)
        {
            var audioSource = childTransform.GetComponent<AudioSource>();
            _audioSources.Add(audioSource);
        }
    }

    public void SingleSound(AudioSource audioSource)
    {
        foreach (var currentAudioSource in _audioSources)
            if (currentAudioSource.isPlaying && currentAudioSource != audioSource)
                currentAudioSource.Stop();
    }
}
