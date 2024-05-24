using UnityEngine;

[RequireComponent(typeof(Renderer),
                  typeof(Collider),
                  typeof(AudioSource))]
public sealed class ObstacleDeactivator : SelectableObject
{
    [SerializeField] private AudioClip _popClip;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.clip = _popClip;

        _renderer.enabled = true;
    }

    protected override void React() => DeactivateObstacle();

    private void DeactivateObstacle()
    {
        _audioSource.Play();

        _renderer.enabled = false;
        _collider.enabled = false;
    }
}
