using System.Collections;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private float _rayLength = 1f;
    [SerializeField, Range(0, 0.5f)] private float _raycastDelay;

    private RaycastHit hit;
    private ISelectable _previousSelectable;
    private ISelectable _currentSelectable;

    private void Start()
    {
        StartCoroutine(Select());
    }

    private IEnumerator Select()
    {
        var waitRaycastDelay = new WaitForSeconds(_raycastDelay);

        while (true)
        {
            Ray ray = new(transform.position, transform.forward);

            if (Physics.Raycast(ray, out hit, _rayLength))
            {
                _currentSelectable = hit.collider.GetComponent<ISelectable>();

                if (_currentSelectable != null && !_currentSelectable.GetSelected())
                {
                    _currentSelectable.SetSelected(true);

                    if (_previousSelectable != null && _currentSelectable != _previousSelectable && _previousSelectable.GetSelected())
                        _previousSelectable.SetSelected(false);

                    _previousSelectable = _currentSelectable;
                }
                else if (_currentSelectable == null && _previousSelectable != null && _previousSelectable.GetSelected())
                {
                    _previousSelectable.SetSelected(false);
                }
            }
            else if (_previousSelectable != null && _previousSelectable.GetSelected())
            {
                _previousSelectable.SetSelected(false);
            }

            yield return waitRaycastDelay;
        }
    }
}
