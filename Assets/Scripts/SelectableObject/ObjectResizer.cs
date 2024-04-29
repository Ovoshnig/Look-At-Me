using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ObjectResizer : SelectableObject
{
    [SerializeField, Range(0, 2)] private int _axis;
    [SerializeField] private float _resizeCoefficient;
    [SerializeField] private float _maxLength;

    [SerializeField] private float _soundChangeDelay;

    [SerializeField] private OneSoundAtTimeProvider _oneSoundAtTimeProvider;
    [SerializeField] private AudioClip _upSoundClip;
    [SerializeField] private AudioClip _downSoundClip;

    [SerializeField] private AudioSource _audioSource;

    private Vector3 _initialScale;
    private Vector3 _initialPosition;

    private void OnValidate() => _audioSource ??= GetComponent<AudioSource>();
    
    private void Start()
    {
        _initialScale = transform.localScale;
        _initialPosition = transform.localPosition;
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;

        if (_isStartCoroutineAllowed)
        {
            StartCoroutine(Resize());
            _isStartCoroutineAllowed = false;
        }
    }

    private IEnumerator Resize()
    {
        int sign = default;

        while (true) 
        {
            if (_isSelect && transform.localScale[_axis] < _maxLength)
            {
                if (sign != 1)
                {
                    sign = 1;

                    StartCoroutine(SoftStopSound());
                    yield return new WaitForSeconds(3 * _soundChangeDelay);

                    _audioSource.clip = _upSoundClip;
                    _audioSource.Play();

                    _oneSoundAtTimeProvider.SingleSound(_audioSource);
                }
            }
            else if (!_isSelect && transform.localScale[_axis] > _initialScale[_axis])
            {
                if (sign != -1)
                {
                    sign = -1;

                    StartCoroutine(SoftStopSound());
                    yield return new WaitForSeconds(3 * _soundChangeDelay);

                    _audioSource.clip = _downSoundClip;
                    _audioSource.Play();

                    _oneSoundAtTimeProvider.SingleSound(_audioSource);
                }
            }
            else
            {
                if (_isSelect)
                {
                    Vector3 scale = _initialScale;
                    scale[_axis] = _maxLength;
                    transform.localScale = scale;

                    Vector3 position = _initialPosition;
                    position[_axis] += 0.5f * Mathf.Sign(_resizeCoefficient) * (_maxLength - _initialScale[_axis]);
                    transform.localPosition = position;
                }
                else
                {
                    transform.localScale = _initialScale;

                    transform.localPosition = _initialPosition;
                }

                StartCoroutine(SoftStopSound());

                _isStartCoroutineAllowed = true;

                yield break;
            }

            float difference = _resizeCoefficient * Time.deltaTime;

            Vector3 currentScale = transform.localScale;
            currentScale[_axis] += sign * Mathf.Abs(difference);
            transform.localScale = currentScale;

            Vector3 currentPosition = transform.localPosition;
            currentPosition[_axis] += sign * 0.5f * difference;
            transform.localPosition = currentPosition;

            yield return null;
        }
    }

    public IEnumerator SoftStopSound()
    {
        var waitSoundChangeDelay = new WaitForSeconds(_soundChangeDelay);

        _audioSource.volume = 0;
        yield return waitSoundChangeDelay;
        _audioSource.Stop();
        yield return waitSoundChangeDelay;
        _audioSource.volume = 1;
    }
}
