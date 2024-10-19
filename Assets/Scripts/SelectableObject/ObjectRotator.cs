using UnityEngine;

[RequireComponent(typeof(Rigidbody),
                  typeof(AudioSource))]
public sealed class ObjectRotator : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField, Range(-1, 1)] private int _direction;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AudioClip[] _rotationClips;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private void OnValidate()
    {
        if (_direction == 0)
            _direction = 1;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        int clipIndex = Random.Range(0, _rotationClips.Length);
        _audioSource.clip = _rotationClips[clipIndex];
        _audioSource.Play();
    }

    protected override void React()
    {
        if (IsSelected)
            _audioSource.Pause();
        else
            _audioSource.UnPause();
    }

    private void FixedUpdate()
    {
        Vector3 angularVelocity = Vector3.zero;
        angularVelocity[_axis] = _direction * _rotationSpeed;
        _rigidbody.angularVelocity = IsSelected ? Vector3.zero : angularVelocity;
    }
}
