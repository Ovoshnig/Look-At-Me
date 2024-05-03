using UnityEngine;
using Zenject;

public sealed class ObjectRepainter : SelectableObject
{
    [SerializeField] private Material _paintMaterial;
    [SerializeField] private Renderer _renderer;
    
    [Inject] private readonly ObjectsInCorrectStatesCounter _counter;

    private void Start() => _counter.IncreaseObjectsCount();
    
    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect && _renderer.material.color != _paintMaterial.color)
        {
            _renderer.material = _paintMaterial;

            _counter.IncreaseCorrectObjectsCount();
        }
    }
}
