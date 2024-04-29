using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public sealed class ObjectRotator : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField, Range(-1, 1)] private int _direction;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private AudioClip[] _rotationClips;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioSource _audioSource;

    private int _clipIndex;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        if (_direction == 0)
            _direction = 1;
    }

    private void Start()
    {
        _clipIndex = Random.Range(0, _rotationClips.Length);
        _audioSource.clip = _rotationClips[_clipIndex];
        _audioSource.volume = 0.16f;
        _audioSource.Play();
    }

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
            _audioSource.Pause();
        else
            _audioSource.UnPause();
    }

    private void FixedUpdate()
    {
        Vector3 angularVelocity = Vector3.zero;
        angularVelocity[_axis] = _direction * _rotationSpeed;
        _rigidbody.angularVelocity = IsSelect ? Vector3.zero : angularVelocity;
    }
}
