using UnityEngine;
using Zenject;

public class TouchTriggerCompletist : MonoBehaviour
{
    private LevelSwitch _levelSwitch;
    private const string PlayerTag = "Player";

    [Inject]
    private void Construct(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
            _levelSwitch.TryLoadNextLevelFirstTime();
    }
}
