using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public sealed class DuringPreviewSelector : SelectableObject
{
    [SerializeField] private float _lookingTime;

    private DuringObjectSelectionCompletist _completist;
    private CancellationTokenSource _cts;

    [Inject]
    private void Construct(DuringObjectSelectionCompletist completist)
    {
        _completist = completist;
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

    protected override void React()
    {
        if (IsSelected)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            ActivateDelayed(_cts.Token).Forget();
        }
        else
        {
            CancelToken();
        }
    }

    private async UniTask ActivateDelayed(CancellationToken token)
    {
        await UniTask.WaitForSeconds(_lookingTime, cancellationToken: token);

        _completist.TimerActivate(this);
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
