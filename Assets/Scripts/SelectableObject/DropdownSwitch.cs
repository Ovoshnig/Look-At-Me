using System.Threading;
using System.Threading.Tasks;
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

    private void OnValidate() => _completist = FindObjectOfType<ObjectsInCorrectStatesCounter>();

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
            _ = SwitchDropdown(_cts.Token);
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

    private async Task SwitchDropdown(CancellationToken token)
    {
        int delayTime = (int)(1000 * _switchDelay);
        int stepTime = 100;

        while (IsSelect)
        {
            _index = (_index + 1) % _dropdown.options.Count;
            _dropdown.value = _index;

            bool isCorrect = _index == _correctIndex;

            if (isCorrect)
                _completist.IncreaseNumberOfCorrectObjects();
            else if (_isDecreaseAllowed)
                _completist.DecreaseNumberOfCorrectObjects();

            _isDecreaseAllowed = isCorrect;

            for (int elapsed = 0; elapsed < delayTime; elapsed += stepTime)
            {
                if (token.IsCancellationRequested)
                    return;

                await Task.Delay(stepTime);
            }
        }
    }
}
