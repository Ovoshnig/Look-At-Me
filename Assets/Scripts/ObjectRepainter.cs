using UnityEngine;

public class ObjectRepainter : SelectableObject
{
    [SerializeField] private Material _paintMaterial;
    [SerializeField] private Renderer _renderer;

    private ObjectsInCorrectStatesCounter _completist;

    private void Awake()
    {
        _completist = GameObject.Find("LevelManager").GetComponent<ObjectsInCorrectStatesCounter>();
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;

        if (_isSelect && _renderer.material.color != _paintMaterial.color)
        {
            _renderer.material = _paintMaterial;

            _completist.IncreaseNumberOfCorrectObjects();
        }
    }
}
