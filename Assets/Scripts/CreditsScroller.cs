using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent (typeof(RectTransform))]
public class CreditsScroller : MonoBehaviour
{
    [SerializeField] private float _startDelay;
    [SerializeField] private float _endDelay;
    [SerializeField] private float _scrollSpeed;

    private TMP_Text _TMP_Text;
    private RectTransform _rectTransform;

    private readonly string _filePath = "Assets/Resources/Documents/Credits.txt";

    private string[] _lines;

    private void Awake()
    {
        _TMP_Text = GetComponent<TMP_Text>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        try
        {
            _lines = File.ReadAllLines(_filePath);

            StartCoroutine(ScrollText());
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

    private IEnumerator ScrollText()
    {
        Vector3 position = transform.localPosition;
        position.z = 1000f;
        transform.localPosition = position;

        _TMP_Text.text = string.Join("\n", _lines);

        yield return new WaitForSeconds(_startDelay);

        float yDelta = 1080f + 0.5f * _rectTransform.sizeDelta.y;
        position.y = -yDelta;
        position.z = 0f;
        transform.localPosition = position;

        while (true)
        {
            Vector3 positionDifference = new(0, _scrollSpeed * Time.deltaTime, 0);
            transform.localPosition += positionDifference;

            if (transform.localPosition.y > yDelta)
            {
                yield return new WaitForSeconds(_endDelay);

                SceneManager.LoadScene(0);
            }

            yield return null;
        }
    }
}
