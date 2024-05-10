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

    private void OnValidate()
    {
        if (_renderer == null)
        {
            _renderer = GetComponent<Renderer>();
        }
        if (_collider == null)
        {
            _collider = GetComponent<Collider>();
        }
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.clip = _popClip;
        }
    }

    private void Awake() => _renderer.enabled = true;

    protected override void React() => DeactivateObstacle();

    private void DeactivateObstacle()
    {
        _audioSource.Play();

        _renderer.enabled = false;
        _collider.enabled = false;
    }
}
