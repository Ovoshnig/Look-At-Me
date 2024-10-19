using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TouchTriggerCompletist : MonoBehaviour
{
    private SceneSwitch _sceneSwitch;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch)
    {
        _sceneSwitch = sceneSwitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<FPSController>(out _))
            _sceneSwitch.TryLoadNextLevelFirstTime().Forget();
    }
}
