using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class ObjectResizer : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField, Range(-1, 1)] private int _direction;
    [SerializeField] private float _resizeSpeed;
    [SerializeField] private float _maxLength;

    [SerializeField] private AudioClip _upSoundClip;
    [SerializeField] private AudioClip _downSoundClip;

    [SerializeField] private OneSoundAtTimeProvider _oneSoundAtTimeProvider;

    private Vector3 _initialScale;
    private Vector3 _initialPosition;
    private AudioSource _audioSource;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    private void OnValidate()
    {
        _oneSoundAtTimeProvider ??= FindObjectOfType<OneSoundAtTimeProvider>();

        if (_direction == 0)
            _direction = 1;
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _initialScale = transform.localScale;
        _initialPosition = transform.localPosition;
    }

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();

        Resize(IsSelect, _cts.Token);
    }

    private async void Resize(bool increasing, CancellationToken token)
    {
        AudioClip targetClip = increasing ? _upSoundClip : _downSoundClip;
        if (_audioSource.clip != targetClip)
        {
            _audioSource.clip = targetClip;
            _audioSource.Play();
            _oneSoundAtTimeProvider.SingleSound(_audioSource);
        }

        try
        {
            while ((increasing ? transform.localScale[_axis] < _maxLength :
                transform.localScale[_axis] > _initialScale[_axis]) && 
                !token.IsCancellationRequested)
            {
                AdjustScaleAndPosition(increasing ? 1 : -1);
                await Task.Delay(10, token);
            }

            _audioSource.Stop();
        }
        catch (TaskCanceledException)
        {
            // Задача была отменена
            // Вернуть объект в исходное состояние или в состояние, соответствующее текущему выбору, если это необходимо
        }
        catch
        {

        }
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
}