using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TouchTriggerCompletist : MonoBehaviour
{
    private LevelSwitch _levelSwitch;

    [Inject]
    private void Construct(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FPSController>(out _))
            _levelSwitch.TryLoadNextLevelFirstTime().Forget();
    }
}
