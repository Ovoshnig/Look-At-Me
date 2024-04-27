using System.Collections;
using UnityEngine;

public class DuringPrewievSelector : SelectableObject
{
    private DuringObjectSelectionCompletist _duringLookingAtPreviewCompletion;

    private float _lookingTime = 0f;

    private void Awake()
    {
        _duringLookingAtPreviewCompletion = GameObject.Find("LevelManager").GetComponent<DuringObjectSelectionCompletist>();
    }

    public override void SetSelected(bool isSelect)
    {
        _isSelect = isSelect;
        if (_isSelect && _isStartCoroutineAllowed)
        {
            StartCoroutine(SelectingTimer());
            _isStartCoroutineAllowed = false;
        }
    }

    private IEnumerator SelectingTimer()
    {
        while (_isSelect)
        {
            _lookingTime += Time.deltaTime;
            if (_lookingTime >= 5f)
            {
                _duringLookingAtPreviewCompletion.TimerActivate(this);
                _lookingTime = 0;
            }
            yield return null;
        }

        _isStartCoroutineAllowed = true;
    }
}
