using UnityEngine;

public class TouchTriggerCompletist : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PauseMenuHandler pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();
            pauseMenuHandler.LoadNextLevel(true);
        }
    }
}
