using UnityEngine;

public class DuringObjectSelectionCompletist : MonoBehaviour
{
    [SerializeField] private DuringPreviewSelector _correctSelector;
    [SerializeField] private DuringPreviewSelector _potopSelector;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject[] _anvils;
    [SerializeField] private GameObject _book;

    private int _randomAnvilIndex;
    private Vector3 _objectSpawnPosition;

    public void TimerActivate(DuringPreviewSelector selector)
    {
        if (selector == _correctSelector)
        {
            PauseMenuHandler pauseMenuHandler = FindObjectOfType<PauseMenuHandler>();
            pauseMenuHandler.LoadNextLevel(true);
        }
        else if (selector == _potopSelector)
        {
            _objectSpawnPosition = _playerTransform.position + new Vector3(0, 10f, 0);
            Instantiate(_book, _objectSpawnPosition, Quaternion.Euler(-90f, 0f, 0f));
        }
        else
        {
            _randomAnvilIndex = Random.Range(0, _anvils.Length);
            _objectSpawnPosition = _playerTransform.position + new Vector3(0, 10f, 0);
            Instantiate(_anvils[_randomAnvilIndex], _objectSpawnPosition, Quaternion.Euler(-90f, 0f, 0f));
        }
    }
}
