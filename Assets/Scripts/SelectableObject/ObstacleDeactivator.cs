using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class ObstacleDeactivator : SelectableObject
{
    [SerializeField] private AudioClip _popClip;

    private Renderer _renderer;
    private Collider _collider;

    private AudioSource _audioSource;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;

        _collider = GetComponent<Collider>();

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _popClip;
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = false;

        _audioSource.Play();

        _renderer.enabled = false;
        _collider.enabled = false;
    }
}
