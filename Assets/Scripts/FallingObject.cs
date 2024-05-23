using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FallingObject : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    private void OnCollisionEnter(Collision collision)
    {
        float pitch = Random.Range(0.7f, 1.3f);
        _audioSource.pitch = pitch;
        _audioSource.Play();
    }
}
