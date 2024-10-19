using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(TMP_Text),
                  typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;
    [SerializeField] private float _scrollSpeed;

    private const string ResourcePath = "Documents/Credits";
    private const float ScreenHeight = 1080f;
    private const float Half = 0.5f;
    private const float SpeedUpCoefficient = 2f;

    private TMP_Text _TMP_Text;
    private RectTransform _rectTransform;
    private string[] _lines;
    private CancellationTokenSource _cts = new();
    private GameState _gameState;
    private SceneSwitch _sceneSwitch;
    private PlayerInput _playerInput;
    private bool _isPaused;
    private bool _isSpeeded;

    [Inject]
    private void Construct(GameState gameState, SceneSwitch sceneSwitch)
    {
        _gameState = gameState;
        _sceneSwitch = sceneSwitch;

        _playerInput = new PlayerInput();
        _playerInput.Credits.SpeedUp.performed += ScrollSpeedUpPerform;
        _playerInput.Credits.SpeedUp.canceled += ScrollSpeedUpCancel;
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

        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _gameState.GamePaused -= Pause;
        _gameState.GameUnpaused -= Unpause;

        _playerInput.Disable();

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
            TextAsset creditsTextAsset = Resources.Load<TextAsset>(ResourcePath);
            if (creditsTextAsset != null)
            {
                _lines = creditsTextAsset.text.Split(new[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
                ScrollText(_cts.Token).Forget();
            }
            else
            {
                Debug.LogError("Credits file not found in Resources.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private async UniTaskVoid ScrollText(CancellationToken token)
    {
        Camera.main.farClipPlane = 100f;

        _TMP_Text.text = string.Join("\n", _lines);

        await UniTask.WaitForSeconds(_startDelay, cancellationToken: token);

        Vector3 position = transform.localPosition;
        float yDelta = ScreenHeight + Half * _rectTransform.sizeDelta.y;
        position.y = -yDelta;
        transform.localPosition = position;

        Camera.main.farClipPlane = 100.1f;

        while (transform.localPosition.y < yDelta)
        {
            Vector3 positionDifference = Vector3.zero;
            positionDifference.y = _scrollSpeed * Time.deltaTime;
            positionDifference.y *= _isSpeeded ? SpeedUpCoefficient : 1f;
            transform.localPosition += positionDifference;

            await UniTask.Yield(cancellationToken: token);

            if (_isPaused)
                await UniTask.WaitUntil(() => _isPaused == false);
        }

        await UniTask.WaitForSeconds(_endDelay, cancellationToken: token);
        _sceneSwitch.LoadLevel(0).Forget();
    }

    private void ScrollSpeedUpPerform(InputAction.CallbackContext _) => _isSpeeded = true;

    private void ScrollSpeedUpCancel(InputAction.CallbackContext _) => _isSpeeded = false;

    private void Pause() => _isPaused = true;

    private void Unpause() => _isPaused = false;
}
