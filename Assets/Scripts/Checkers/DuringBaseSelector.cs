using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[SelectionBase]
public sealed class DuringBaseSelector : SelectableObject
{
    [SerializeField] private float _lookingTime = 1;
    [SerializeField] private BaseSelector _baseSelector;

    private CancellationTokenSource _cts;

    private void Awake() => _baseSelector = FindFirstObjectByType<BaseSelector>();

    private void OnDisable() => CancelToken();

    protected override void React()
    {
        if (IsSelected)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            WaitDelay(token).Forget();
        }
        else
        {
            CancelToken();
        }
    }

    private async UniTaskVoid WaitDelay(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lookingTime, cancellationToken: token);
        _baseSelector.SelectBase(gameObject.name);
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
