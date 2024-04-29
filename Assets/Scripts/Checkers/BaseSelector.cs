using UnityEngine;

[RequireComponent(typeof(CheckersLogic))]
[RequireComponent(typeof(CheckersVisualizer))]
public class BaseSelector : SelectableObject
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _basesStartSet;

    [SerializeField] private CheckersLogic _checkersLogic;
    [SerializeField] private CheckersVisualizer _visualizer;

    private void OnValidate()
    {
        _checkersLogic ??= GetComponent<CheckersLogic>();
        _visualizer ??= GetComponent<CheckersVisualizer>();
    }

    private void Start()
    {
        _canvas.SetActive(true);
        _basesStartSet.SetActive(true);
    }

    public void SelectBase(string chosenBaseName)
    {
        _visualizer.ChooseFigure(chosenBaseName);
        StartCoroutine(_checkersLogic.StartPlacement());

        _canvas.SetActive(false);
        _basesStartSet.SetActive(false);
    }
}
