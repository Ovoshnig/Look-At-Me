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
    private CancellationTokenSource _cts;
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
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            Repaint(_cts.Token).Forget();
        }
        else
        {
            CancelToken();
        }
    }

    private async UniTaskVoid Repaint(CancellationToken token)
    {
        while (IsSelected)
        {
            _renderer.material = _materials[_index];
            bool isCorrectMaterial = _materials[_index] == _correctMaterial;

            if (isCorrectMaterial)
                _counter.IncreaseCorrectObjectsCount();
            else if (_isDecreaseAllowed)
                _counter.DecreaseCorrectObjectsCount();

            _isDecreaseAllowed = isCorrectMaterial;

            _index = (_index + 1) % _materials.Length;

            await UniTask.WaitForSeconds(_repaintDelay, cancellationToken: token);
        }
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
