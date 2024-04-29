using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(BaseSelector))]
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

    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            SelectingTimer(_cts.Token).Forget();
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

    private async UniTaskVoid SelectingTimer(CancellationToken token)
    {
        int delayTime = (int)(1000 * _lookingTime);
        await UniTask.Delay(delayTime, cancellationToken: token);

        _baseSelector.SelectBase(gameObject.name);
    }
}
