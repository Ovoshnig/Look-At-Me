using Cysharp.Threading.Tasks;
using Zenject;

public class ObjectsInCorrectStatesCounter
{
    private LevelSwitch _levelSwitch;

    [Inject]
    private ObjectsInCorrectStatesCounter(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
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
            _levelSwitch.TryLoadNextLevelFirstTime().Forget();
    }
}
