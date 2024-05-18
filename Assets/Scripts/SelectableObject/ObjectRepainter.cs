using UnityEngine;
using Zenject;

public sealed class ObjectRepainter : SelectableObject
{
    [SerializeField] private Material _paintMaterial;
    [SerializeField] private Renderer _renderer;
    
    private ObjectsInCorrectStatesCounter _counter;

    [Inject]
    private void Construct(ObjectsInCorrectStatesCounter counter)
    {
        _counter = counter;
    }

    private void Start() => _counter.IncreaseObjectsCount();
    
    protected override void React()
    {
        if (IsSelected && _renderer.material.color != _paintMaterial.color)
        {
            _renderer.material = _paintMaterial;

            _counter.IncreaseCorrectObjectsCount();
        }
    }
}
