using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public sealed class ObstacleDeactivator : SelectableObject
{
    [SerializeField] private AudioClip _popClip;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private AudioSource _audioSource;

    private void OnValidate()
    {
        _renderer ??= GetComponent<Renderer>();
        _collider ??= GetComponent<Collider>();
        _audioSource ??= GetComponent<AudioSource>();

        _audioSource.clip = _popClip;
    }

    private void Awake() => _renderer.enabled = true;
    
    public override void SetSelected(bool isSelect)
    {
        IsSelect = false;

        _audioSource.Play();

        _renderer.enabled = false;
        _collider.enabled = false;
    }
}
