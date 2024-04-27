using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ObjectRotator : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField, Range(-1, 1)] private int _rotationDirection;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private AudioClip[] _rotationClips;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private int _clipIndex;

    private void OnValidate()
    {
        if (_rotationDirection == 0)
            _rotationDirection = 1;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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
        _isSelect = isSelect;

        if (_isSelect)
            _audioSource.Pause();
        else
            _audioSource.UnPause();
    }

    private void FixedUpdate()
    {
        Vector3 angularVelocity = Vector3.zero;
        angularVelocity[_axis] = _rotationDirection * _rotationSpeed;
        _rigidbody.angularVelocity = _isSelect ? Vector3.zero : angularVelocity;
    }
}
