using UnityEngine;

public class TouchTrigger—ompletist : MonoBehaviour
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
