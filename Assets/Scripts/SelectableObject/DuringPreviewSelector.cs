using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public sealed class DuringPreviewSelector : SelectableObject
{
    [SerializeField] private float _lookingTime;
    [SerializeField] private DuringObjectSelectionCompletist _duringLookingAtPreviewCompletion;

    private CancellationTokenSource _cts;

    private void OnValidate() => _duringLookingAtPreviewCompletion ??= FindObjectOfType<DuringObjectSelectionCompletist>();

    private void OnDisable()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
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

        _duringLookingAtPreviewCompletion.TimerActivate(this);
    }
}
