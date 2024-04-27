using System.Collections;
using UnityEngine;

public class ObjectCyclicRepainter : SelectableObject
{
    [SerializeField] private float _repaintDelay;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material[] _materials;
    [SerializeField] private Material _correctMaterial;

    private ObjectsInCorrectStatesCounter _repaintObjects—ompletist;

    private int _index;

    bool _isDecreaseAllowed = false;

    private void Awake()
    {
        _repaintObjects—ompletist = GameObject.Find("LevelManager").GetComponent<ObjectsInCorrectStatesCounter>();
    }

    private void Start()
    {
        _index = Random.Range(0, _materials.Length);
        _renderer.material = _materials[_index];

        if (_materials[_index] == _correctMaterial)
        {
            _repaintObjects—ompletist.IncreaseNumberOfCorrectObjects();
            _isDecreaseAllowed = true;
        }
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;
        if (_isSelect && _isStartCoroutineAllowed)
        {
            StartCoroutine(RepaintRoutine());
            _isStartCoroutineAllowed = false;
        }
    }

    public IEnumerator RepaintRoutine()
    {
        var waitRepaintDelay = new WaitForSeconds(_repaintDelay);

        while (_isSelect)
        {
            _renderer.material = _materials[_index];

            if (_materials[_index] == _correctMaterial)
            {
                _repaintObjects—ompletist.IncreaseNumberOfCorrectObjects();
                _isDecreaseAllowed = true;
            }
            else if (_isDecreaseAllowed)
            {
                _repaintObjects—ompletist.DecreaseNumberOfCorrectObjects();
                _isDecreaseAllowed = false;
            }

            _index = (_index + 1) % _materials.Length;
            yield return waitRepaintDelay;
        }

        _isStartCoroutineAllowed = true;
    }
}
