using UnityEngine;

[RequireComponent(typeof(CheckersLogic),
                 (typeof(CheckersVisualizer)))]
public class BaseSelector : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _basesStartSet;
    [SerializeField] private CheckersLogic _checkersLogic;
    [SerializeField] private CheckersVisualizer _visualizer;

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

    public void SelectBase(string chosenBaseName)
    {
        _visualizer.ChooseFigure(chosenBaseName);
        _checkersLogic.StartPlacement().Forget();

        _canvas.SetActive(false);
        _basesStartSet.SetActive(false);
    }
}
