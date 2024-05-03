using UnityEngine;
using Zenject;

public class TouchTriggerCompletist : MonoBehaviour
{
    [Inject] private readonly PauseMenuHandler _pauseMenuHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _pauseMenuHandler.LoadNextLevel(true);
    }
}
