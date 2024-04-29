using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public sealed class DropdownSwitch : SelectableObject
{
    [SerializeField] private float _switchDelay;
    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] private int _correctIndex;
    [SerializeField] private ObjectsInCorrectStatesCounter _completist;

    private int _index = 0;
    private bool _isDecreaseAllowed = false;
    private CancellationTokenSource _cts;

    private void OnValidate()
    {
        if (_completist == null)
            _completist = FindObjectOfType<ObjectsInCorrectStatesCounter>();
    }

    private void OnDisable()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private void Start()
    {
        if (_dropdown.value == _correctIndex)
        {
            _completist.IncreaseNumberOfCorrectObjects();
            _isDecreaseAllowed = true;
        }
    }

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            SwitchDropdown(_cts.Token).Forget();
        }
        else
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
    }

    private async UniTaskVoid SwitchDropdown(CancellationToken token)
    {
        int delayTime = (int)(1000 * _switchDelay);

        while (IsSelect)
        {
            await UniTask.Delay(delayTime, cancellationToken: token);

            _index = (_index + 1) % _dropdown.options.Count;
            _dropdown.value = _index;

            bool isCorrect = _index == _correctIndex;

            if (isCorrect)
                _completist.IncreaseNumberOfCorrectObjects();
            else if (_isDecreaseAllowed)
                _completist.DecreaseNumberOfCorrectObjects();

            _isDecreaseAllowed = isCorrect;
        }
    }
}
