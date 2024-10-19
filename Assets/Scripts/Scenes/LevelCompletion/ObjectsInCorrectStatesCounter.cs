using Cysharp.Threading.Tasks;
using Zenject;

public class ObjectsInCorrectStatesCounter
{
    private SceneSwitch _sceneSwitch;

    [Inject]
    private ObjectsInCorrectStatesCounter(SceneSwitch sceneSwitch)
    {
        _sceneSwitch = sceneSwitch;
    }

    private uint _objectsCount = 0;
    private uint _correctObjectsCount = 0;

    public void IncreaseObjectsCount() => _objectsCount++;

    public void IncreaseCorrectObjectsCount()
    {
        _correctObjectsCount++;
        CheckIsComplete();
    }

    public void DecreaseCorrectObjectsCount() => _correctObjectsCount--;

    private void CheckIsComplete() 
    {
        if (_correctObjectsCount == _objectsCount)
            _sceneSwitch.TryLoadNextLevelFirstTime().Forget();
    }
}
