using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TMP_Text),
                  typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private TMP_Text _TMP_Text;
    [SerializeField] private RectTransform _rectTransform;

    private readonly string _filePath = "Assets/Resources/Documents/Credits.txt";
    private string[] _lines;
    private CancellationTokenSource _cts = new();

    private void OnValidate()
    {
        if (_TMP_Text == null)
            _TMP_Text = GetComponent<TMP_Text>();
        if (_rectTransform == null)
            _rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
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
            _lines = File.ReadAllLines(_filePath);

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
        }

        int endDelayTime = (int)(1000 * _endDelay);
        await UniTask.Delay(endDelayTime, cancellationToken: token);

        SceneManager.LoadScene(0);
    }
}
