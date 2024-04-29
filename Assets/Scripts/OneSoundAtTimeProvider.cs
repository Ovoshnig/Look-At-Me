using System.Collections.Generic;
using UnityEngine;

public class OneSoundAtTimeProvider : MonoBehaviour
{
    [SerializeField] private Transform _objectsTransform;

    [SerializeField] private List<AudioSource> _audioSources = new();
    [SerializeField] private List<ObjectResizer> _objectResizers = new();

    private void Awake()
    {
        foreach (Transform childTransform in _objectsTransform)
        {
            var audioSource = childTransform.gameObject.GetComponent<AudioSource>();
            _audioSources.Add(audioSource);

            var objectResizer = childTransform.GetComponent<ObjectResizer>();
            _objectResizers.Add(objectResizer);
        }
    }

    public void SingleSound(AudioSource audioSource)
    {
        for (int i = 0; i < _audioSources.Count; i++)
            if (_audioSources[i].isPlaying && _audioSources[i] != audioSource)
                _audioSources[i].Stop();
    }
}
