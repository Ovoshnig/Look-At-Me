using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public sealed class ObjectCyclicRepainter : SelectableObject
{
    [SerializeField] private float _repaintDelay;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _correctMaterial;

    [SerializeField] private ObjectsInCorrectStatesCounter _repaintObjectsCompletist;

    private int _index;
    private bool _isDecreaseAllowed = false;
    private CancellationTokenSource _cts;

    private void OnValidate() => _repaintObjectsCompletist = FindObjectOfType<ObjectsInCorrectStatesCounter>();
    
    private void Start()
    {
        _index = Random.Range(0, _materials.Length);
        _renderer.material = _materials[_index];

        if (_materials[_index] == _correctMaterial)
        {
            _repaintObjectsCompletist.IncreaseNumberOfCorrectObjects();
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
            _ = Repaint(_cts.Token);
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

    private async Task Repaint(CancellationToken token)
    {
        int delayTime = (int)(1000 * _repaintDelay);
        int stepTime = 100;

        while (IsSelect)
        {
            _renderer.material = _materials[_index];
            bool isCorrectMaterial = _materials[_index] == _correctMaterial;

            if (isCorrectMaterial)
                _repaintObjectsCompletist.IncreaseNumberOfCorrectObjects();
            else if (_isDecreaseAllowed)
                _repaintObjectsCompletist.DecreaseNumberOfCorrectObjects();

            _isDecreaseAllowed = isCorrectMaterial;

            _index = (_index + 1) % _materials.Length;

            for (int elapsed = 0; elapsed < delayTime; elapsed += stepTime)
            {
                if (token.IsCancellationRequested)
                    return;

                await Task.Delay(stepTime);
            }
        }
    }
}
