using UnityEngine;
using Zenject;

public class DuringObjectSelectionCompletist : MonoBehaviour
{
    [SerializeField] private DuringPreviewSelector _correctSelector;
    [SerializeField] private DuringPreviewSelector _potopSelector;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject[] _anvils;
    [SerializeField] private GameObject _book;

    private LevelSwitch _levelSwitch;

    [Inject]
    private void Construct(LevelSwitch levelSwitch)
    {
        _levelSwitch = levelSwitch;
    }

    private Vector3 _objectSpawnPosition;

    public void TimerActivate(DuringPreviewSelector selector)
    {
        if (selector == _correctSelector)
        {
            _levelSwitch.TryLoadNextLevelFirstTime();
        }
        else if (selector == _potopSelector)
        {
            _objectSpawnPosition = _playerTransform.position + new Vector3(0, 10f, 0);
            Instantiate(_book, _objectSpawnPosition, Quaternion.Euler(-90f, 0f, 0f));
        }
        else
        {
            int index = Random.Range(0, _anvils.Length);
            _objectSpawnPosition = _playerTransform.position + new Vector3(0, 10f, 0);
            Instantiate(_anvils[index], _objectSpawnPosition, Quaternion.Euler(-90f, 0f, 0f));
        }
    }
}
