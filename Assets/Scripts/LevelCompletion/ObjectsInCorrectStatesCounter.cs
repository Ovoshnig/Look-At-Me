using UnityEngine;
using Zenject;

public class ObjectsInCorrectStatesCounter
{
    [Inject] private readonly PauseMenuHandler _menuHandler;

    private uint _objectsCount = 0;
    private uint _correctObjectsCount = 0;

    public void IncreaseObjectsCount()
    {
        _objectsCount++;
    }

    public void IncreaseCorrectObjectsCount()
    {
        _correctObjectsCount++;
        CheckIsComplete();
    }

    public void DecreaseCorrectObjectsCount()
    {
        _correctObjectsCount--;
    }

    private void CheckIsComplete() 
    {
        if (_correctObjectsCount == _objectsCount)
            _menuHandler.LoadNextLevel(true);
    }
}
