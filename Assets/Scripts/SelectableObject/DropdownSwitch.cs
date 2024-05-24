using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using Zenject;

public sealed class DropdownSwitch : SelectableObject
{
    [SerializeField] private float _switchDelay;
    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] private int _correctIndex;

    private ObjectsInCorrectStatesCounter _counter;
    private CancellationTokenSource _cts;
    private int _index = 0;
    private bool _isDecreaseAllowed = false;

    [Inject]
    private void Construct(ObjectsInCorrectStatesCounter counter)
    {
        _counter = counter;
    }

    private void OnDisable() => CancelToken();

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private void Awake() => _counter.IncreaseObjectsCount();

    private void Start()
    {
        if (_dropdown.value == _correctIndex)
        {
            _counter.IncreaseCorrectObjectsCount();
            _isDecreaseAllowed = true;
        }
    }

    protected override void React()
    {
        if (IsSelected)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            SwitchDropdown(_cts.Token).Forget();
        }
        else
        {
            CancelToken();
        }
    }

    private async UniTaskVoid SwitchDropdown(CancellationToken token)
    {
        while (IsSelected)
        {
            await UniTask.WaitForSeconds(_switchDelay, cancellationToken: token);

            _index = (_index + 1) % _dropdown.options.Count;
            _dropdown.value = _index;

            bool isCorrect = _index == _correctIndex;

            if (isCorrect)
                _counter.IncreaseCorrectObjectsCount();
            else if (_isDecreaseAllowed)
                _counter.DecreaseCorrectObjectsCount();

            _isDecreaseAllowed = isCorrect;
        }
    }
}
