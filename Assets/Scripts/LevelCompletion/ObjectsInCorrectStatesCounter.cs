using UnityEngine;

public class ObjectsInCorrectStatesCounter : MonoBehaviour
{
    [SerializeField] private Transform _objectsTransform;

    private uint _objectsCount;
    private uint _correctObjectsCount = 0;

    private void Start()
    {
        _objectsCount = (uint)_objectsTransform.childCount;
    }

    public void IncreaseNumberOfCorrectObjects()
    {
        _correctObjectsCount++;
        CheckIsComplete();
    }

    public void DecreaseNumberOfCorrectObjects()
    {
        _correctObjectsCount--;
    }

    private void CheckIsComplete() 
    {
        if (_correctObjectsCount == _objectsCount)
        {
            PauseMenuHandler pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();
            pauseMenuHandler.LoadNextLevel(true);
        }
    }
}
