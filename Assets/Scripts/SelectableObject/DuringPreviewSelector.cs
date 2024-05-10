using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public sealed class DuringPreviewSelector : SelectableObject
{
    [SerializeField] private float _lookingTime;

    [Inject] private readonly DuringObjectSelectionCompletist _completist;

    private CancellationTokenSource _cts;

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
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
    }

    private async UniTask ActivateDelayed(CancellationToken token)
    {
        int delayTime = (int)(1000 * _lookingTime);
        await UniTask.Delay(delayTime, cancellationToken: token);

        _completist.TimerActivate(this);
    }
}
