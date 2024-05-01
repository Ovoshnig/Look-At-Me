using UnityEngine;

public sealed class ObjectRepainter : SelectableObject
{
    [SerializeField] private Material _paintMaterial;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private ObjectsInCorrectStatesCounter _counter;

    private void OnValidate()
    {
        if (_counter == null)
            _counter = FindObjectOfType<ObjectsInCorrectStatesCounter>();
    }
    
    public override void SetSelected(bool isSelect)
    {
        IsSelect = isSelect;

        if (IsSelect && _renderer.material.color != _paintMaterial.color)
        {
            _renderer.material = _paintMaterial;

            _counter.IncreaseNumberOfCorrectObjects();
        }
    }
}
