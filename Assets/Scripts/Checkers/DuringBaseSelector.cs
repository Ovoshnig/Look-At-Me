using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[SelectionBase]
public sealed class DuringBaseSelector : SelectableObject
{
    [SerializeField] private float _lookingTime = 1;
    [SerializeField] private BaseSelector _baseSelector;

    private CancellationTokenSource _cts;

    private void OnValidate()
    {
        if (_baseSelector == null)
            _baseSelector = FindObjectOfType<BaseSelector>();
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

    protected async override void React()
    {
        if (IsSelected)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            await SelectingTimer(_cts.Token);

            _baseSelector.SelectBase(gameObject.name);
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

    private async UniTask SelectingTimer(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lookingTime, cancellationToken: token);
    }
}
