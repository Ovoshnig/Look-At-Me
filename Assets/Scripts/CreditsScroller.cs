using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text),
                  typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;
    [SerializeField] private float _scrollSpeed;

    private const string FilePath = "Assets/Resources/Documents/Credits.txt";

    private TMP_Text _TMP_Text;
    private RectTransform _rectTransform;
    private string[] _lines;
    private CancellationTokenSource _cts = new();
    private GameState _gameState;
    private LevelSwitch _levelSwitch;
    private bool _isPaused;

    [Inject]
    private void Construct(GameState gameState, LevelSwitch levelSwitch)
    {
        _gameState = gameState;
        _levelSwitch = levelSwitch;
    }

    private void Awake()
    {
        _TMP_Text = GetComponent<TMP_Text>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _gameState.GamePaused += Pause;
        _gameState.GameUnpaused += Unpause;
    }

    private void OnDisable()
    {
        _gameState.GamePaused -= Pause;
        _gameState.GameUnpaused -= Unpause;

        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private void Start()
    {
        try
        {
            _lines = File.ReadAllLines(FilePath);

            ScrollText(_cts.Token).Forget();
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private async UniTaskVoid ScrollText(CancellationToken token)
    {
        Vector3 position = transform.localPosition;
        position.z = 1000f;
        transform.localPosition = position;

        _TMP_Text.text = string.Join("\n", _lines);

        int startDelayTime = (int)(1000 * _startDelay);
        await UniTask.Delay(startDelayTime, cancellationToken: token);

        float yDelta = 1080f + 0.5f * _rectTransform.sizeDelta.y;
        position.y = -yDelta;
        position.z = 0f;
        transform.localPosition = position;

        while (transform.localPosition.y < yDelta)
        {
            Vector3 positionDifference = new(0, _scrollSpeed * Time.deltaTime, 0);
            transform.localPosition += positionDifference;

            await UniTask.Yield(cancellationToken: token);

            if (_isPaused)
                await UniTask.WaitUntil(() => _isPaused == false);
        }

        await UniTask.WaitForSeconds(_endDelay, cancellationToken: token);

        _levelSwitch.LoadLevel(0).Forget();
    }

    private void Pause() => _isPaused = true;

    private void Unpause() => _isPaused = false;
}
