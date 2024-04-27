using UnityEngine;

[RequireComponent(typeof(CheckersLogic))]
[RequireComponent(typeof(CheckersVisualizer))]
public class BaseSelector : SelectableObject
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _basesStartSet;

    private CheckersLogic _checkersLogic;
    private CheckersVisualizer _visualizer;

    private void Awake()
    {
        _checkersLogic = GetComponent<CheckersLogic>();
        _visualizer = GetComponent<CheckersVisualizer>();
    }

    private void Start()
    {
        _canvas.SetActive(true);
        _basesStartSet.SetActive(true);
    }

    public void SelectBase(string choosenBaseName)
    {
        _visualizer.ChooseFigure(choosenBaseName);
        StartCoroutine(_checkersLogic.StartPlacement());

        _canvas.SetActive(false);
        _basesStartSet.SetActive(false);
    }
}
