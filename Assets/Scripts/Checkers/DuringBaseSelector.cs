using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[SelectionBase]
public sealed class DuringBaseSelector : SelectableObject
{
    [SerializeField] private float _lookingTime = 1;
    [SerializeField] private BaseSelector _baseSelector;

    private CancellationTokenSource _cts;

    private void OnValidate() => _baseSelector ??= FindObjectOfType<BaseSelector>();

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _ = SelectingTimer(_cts.Token);
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

    private async Task SelectingTimer(CancellationToken token)
    {
        int delayTime = (int)(1000 * _lookingTime);
        int stepTime = 100;
        for (int elapsed = 0; elapsed < delayTime; elapsed += stepTime)
        {
            if (token.IsCancellationRequested)
                return;

            await Task.Delay(stepTime);
        }

        _baseSelector.SelectBase(gameObject.name);
    }
}
