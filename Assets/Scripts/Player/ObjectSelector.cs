using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField] private float _rayLength = 1f;
    [SerializeField, Range(0, 0.5f)] private float _raycastDelay;

    [SerializeField] private LayerMask _selectableLayer;

    private readonly Dictionary<GameObject, ISelectable> _selectableDictionary = new();

    private ISelectable _previousSelectable;
    private ISelectable _currentSelectable;

    private void Awake()
    {
        var selectables = FindObjectsOfType<SelectableObject>();

        foreach (var selectableComponent in selectables)
            if (selectableComponent is MonoBehaviour monoBehaviour)
                _selectableDictionary[monoBehaviour.gameObject] = selectableComponent;
    }

    private void Start() => StartCoroutine(Select());

    private IEnumerator Select()
    {
        var waitRaycastDelay = new WaitForSeconds(_raycastDelay);

        while (true)
        {
            Ray ray = new(transform.position, transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _selectableLayer))
            {
                _selectableDictionary.TryGetValue(hit.collider.gameObject, out _currentSelectable);

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
