using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public sealed class DuringPreviewSelector : SelectableObject
{
    [SerializeField] private float _lookingTime;
    [SerializeField] private DuringObjectSelectionCompletist _duringLookingAtPreviewCompletion;

    private CancellationTokenSource _cts;

    private void OnValidate() => _duringLookingAtPreviewCompletion = FindObjectOfType<DuringObjectSelectionCompletist>();
    
    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _ = ActivateDelayed(_cts.Token);
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

    private async Task ActivateDelayed(CancellationToken token)
    {
        int delayTime = (int)(1000 * _lookingTime);
        int stepTime = 100;
        for (int elapsed = 0; elapsed < delayTime; elapsed += stepTime)
        {
            if (token.IsCancellationRequested)
                return;
            
            await Task.Delay(stepTime);
        }

        _duringLookingAtPreviewCompletion.TimerActivate(this);
    }
}
