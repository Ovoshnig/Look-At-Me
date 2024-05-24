using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public sealed class ObjectResizer : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField, Range(-1, 1)] private int _direction;
    [SerializeField] private float _resizeSpeed;
    [SerializeField] private float _maxLength;
    [SerializeField] private AudioClip _upSoundClip;
    [SerializeField] private AudioClip _downSoundClip;

    private Vector3 _initialScale;
    private AudioSource _audioSource;
    private OneSoundAtTimeProvider _oneSoundAtTimeProvider;
    private CancellationTokenSource _cts = new();

    [Inject]
    private void Construct(OneSoundAtTimeProvider oneSoundAtTimeProvider)
    {
        _oneSoundAtTimeProvider = oneSoundAtTimeProvider;
    }

    private void OnValidate()
    {
        if (_direction == 0)
            _direction = 1;
    }

    private void OnDisable() => CancelToken();

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _initialScale = transform.localScale;
    }

    protected override void React()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        Resize(IsSelected, _cts.Token).Forget();
    }

    private async UniTaskVoid Resize(bool increasing, CancellationToken token)
    {
        AudioClip targetClip = increasing ? _upSoundClip : _downSoundClip;
        if (_audioSource.clip != targetClip)
        {
            _audioSource.clip = targetClip;
            _audioSource.Play();
            _oneSoundAtTimeProvider.SingleSound(_audioSource);
        }

        while ((increasing ? transform.localScale[_axis] < _maxLength :
            transform.localScale[_axis] > _initialScale[_axis]) && 
            !token.IsCancellationRequested)
        {
            AdjustScaleAndPosition(increasing ? 1 : -1);
            await UniTask.Yield(cancellationToken: token);
        }

        _audioSource.Stop();
    }

    private void AdjustScaleAndPosition(int direction)
    {
        float scaleChange = direction * Mathf.Abs(_resizeSpeed * Time.deltaTime);
        Vector3 scale = transform.localScale;
        scale[_axis] += scaleChange;
        transform.localScale = scale;

        float positionChange = _direction * direction * 0.5f * _resizeSpeed * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position[_axis] += positionChange;
        transform.localPosition = position;
    }

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}