using UnityEngine;

[RequireComponent(typeof(CheckersLogic),
                 (typeof(CheckersVisualizer)))]
public class BaseSelector : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _basesStartSet;
    [SerializeField] private CheckersLogic _checkersLogic;
    [SerializeField] private CheckersVisualizer _visualizer;

    private void OnValidate()
    {
        if (_checkersLogic == null)
            _checkersLogic = GetComponent<CheckersLogic>();
        if (_visualizer == null)
            _visualizer = GetComponent<CheckersVisualizer>();
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
