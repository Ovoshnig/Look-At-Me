using System.Collections;
using UnityEngine;

[SelectionBase]
public class DuringBaseSelector : SelectableObject
{
    [SerializeField] private BaseSelector _baseChoice;

    private float _lookingTime = 0f;

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;
        if (_isSelect && _isStartCoroutineAllowed)
        {
            StartCoroutine(SelectingTimer());
            _isStartCoroutineAllowed = false;
        }
        else 
        {
            _lookingTime = 0;
        }
    }

    private IEnumerator SelectingTimer()
    {
        while (_isSelect)
        {
            _lookingTime += Time.deltaTime;
            if (_lookingTime > 5f)
            {
                _baseChoice.SelectBase(gameObject.name);
            }

            yield return null;
        }

        _isStartCoroutineAllowed = true;
    }
}
