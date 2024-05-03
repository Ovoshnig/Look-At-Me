using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Renderer))]
public sealed class ObjectCyclicRepainter : SelectableObject
{
    [SerializeField] private float _repaintDelay;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _correctMaterial;

    [Inject] private readonly ObjectsInCorrectStatesCounter _counter;

    private int _index;
    private bool _isDecreaseAllowed = false;
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

    private void Awake() => _counter.IncreaseObjectsCount();

    private void Start()
    {
        _index = Random.Range(0, _materials.Length);
        _renderer.material = _materials[_index];

        if (_materials[_index] == _correctMaterial)
        {
            _counter.IncreaseCorrectObjectsCount();
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
            Repaint(_cts.Token).Forget();
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

    private async UniTaskVoid Repaint(CancellationToken token)
    {
        int delayTime = (int)(1000 * _repaintDelay);

        while (IsSelect)
        {
            _renderer.material = _materials[_index];
            bool isCorrectMaterial = _materials[_index] == _correctMaterial;

            if (isCorrectMaterial)
                _counter.IncreaseCorrectObjectsCount();
            else if (_isDecreaseAllowed)
                _counter.DecreaseCorrectObjectsCount();

            _isDecreaseAllowed = isCorrectMaterial;

            _index = (_index + 1) % _materials.Length;

            await UniTask.Delay(delayTime, cancellationToken: token);
        }
    }
}
