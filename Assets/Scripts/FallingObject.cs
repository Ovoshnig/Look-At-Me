using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FallingObject : MonoBehaviour
{
    private AudioSource _audioSource;

    private float _randomPitch;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _randomPitch = Random.Range(0.7f, 1.3f);
        _audioSource.pitch = _randomPitch;
        _audioSource.Play();
    }
}
