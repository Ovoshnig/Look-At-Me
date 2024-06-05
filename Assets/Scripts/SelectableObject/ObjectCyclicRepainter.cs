using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Renderer))]
public sealed class ObjectCyclicRepainter : SelectableObject
{
    [SerializeField] private float _repaintDelay;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _correctMaterial;

    private Renderer _renderer;
    private ObjectsInCorrectStatesCounter _counter;
    private CancellationTokenSource _cts = new();
    private int _index;
    private bool _isDecreaseAllowed = false;

    [Inject]
    private void Construct(ObjectsInCorrectStatesCounter counter) => _counter = counter;

    private void OnDisable() => CancelToken();

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _counter.IncreaseObjectsCount();
    }

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

    protected override void React()
    {
        if (IsSelected)
        {
            Repaint().Forget();
        }
        else
        {
            CancelToken();
            _cts = new CancellationTokenSource();
        }
    }

    private async UniTaskVoid Repaint()
    {
        while (IsSelected)
        {
            await UniTask.WaitForSeconds(_repaintDelay, cancellationToken: _cts.Token);

            _index = (_index + 1) % _materials.Length;
            _renderer.material = _materials[_index];
            bool isCorrectMaterial = _materials[_index] == _correctMaterial;

            if (isCorrectMaterial)
                _counter.IncreaseCorrectObjectsCount();
            else if (_isDecreaseAllowed)
                _counter.DecreaseCorrectObjectsCount();

            _isDecreaseAllowed = isCorrectMaterial;
        }
    }

    private void CancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
