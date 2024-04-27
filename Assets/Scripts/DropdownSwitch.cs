using System.Collections;
using TMPro;
using UnityEngine;

public class DropdownSwitch : SelectableObject
{
    [SerializeField] private float _swapDelay;

    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] private int _correctIndex;

    private ObjectsInCorrectStatesCounter _completist;

    private int _index = 0;

    bool _isDecreaseAllowed = false;

    private void Awake()
    {
        _completist = GameObject.Find("LevelManager").GetComponent<ObjectsInCorrectStatesCounter>();
    }

    private void Start()
    {
        if (_dropdown.value == _correctIndex)
        {
            _completist.IncreaseNumberOfCorrectObjects();
            _isDecreaseAllowed = true;
        }
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;

        if (_isSelect && _isStartCoroutineAllowed)
        {
            StartCoroutine(SwapRoutine());
            _isStartCoroutineAllowed = false;
        }
    }

    public IEnumerator SwapRoutine()
    {
        var waitSwapDelay = new WaitForSeconds(_swapDelay);

        while (_isSelect)
        {
            _index = (_index + 1) % _dropdown.options.Count;
            _dropdown.value = _index;

            if (_index == _correctIndex)
            {
                _completist.IncreaseNumberOfCorrectObjects();
                _isDecreaseAllowed = true;
            }
            else if (_isDecreaseAllowed)
            {
                _completist.DecreaseNumberOfCorrectObjects();
                _isDecreaseAllowed = false;
            }

            yield return waitSwapDelay;
        }

        _isStartCoroutineAllowed = true;
    }
}
