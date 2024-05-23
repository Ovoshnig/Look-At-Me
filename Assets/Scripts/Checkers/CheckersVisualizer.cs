using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckersVisualizer : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _figureSize;
    [SerializeField] private List<GameObject> _figurePrefabs;
    [SerializeField] private GameObject _crownPrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _putClips;
    [SerializeField] private AudioClip[] _dragClips;
    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _lossClip;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private float _jumpDuration;
    [SerializeField] private float _jumpHeigh;

    private const float ÑellSize = 2.5f;

    private readonly GameObject[] _playerFigures = new GameObject[2];
    private readonly Transform[,] _figureTransforms = new Transform[8, 8];
    private Transform figureTransform;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Awake() => _audioSource = GetComponent<AudioSource>();

    public void ChooseFigure(string playerFigureName)
    {
        _playerFigures[0] = _figurePrefabs.Find(x => x.name == playerFigureName);
        _figurePrefabs.Remove(_playerFigures[0]);

        int index = Random.Range(0, _figurePrefabs.Count);
        _playerFigures[1] = _figurePrefabs[index];
    }

    private Vector3 IndexesToPosition(int i, int j)
    {
        float iFloat = i + 0.5f;
        float jFloat = j + 0.5f;

        iFloat -= 4f;
        jFloat -= 4f;

        iFloat *= ÑellSize;
        jFloat *= ÑellSize;

        Vector3 position = new(iFloat, 0f, jFloat);
        return position;
    }

    public void PlaceFigure(int i, int j, int index)
    {
        Vector3 position = IndexesToPosition(i, j);
        var newFigure = Instantiate(_playerFigures[index], position, Quaternion.Euler(0, 0, 0));
        newFigure.transform.localScale *= _figureSize;
        _figureTransforms[i, j] = newFigure.transform;

        int clipIndex = Random.Range(0, _putClips.Length);
        _audioSource.clip = _putClips[clipIndex];
        _audioSource.Play();
    }

    public async UniTask MakeFigureMove(List<int> turnIndex) 
    {
        var (i, j, iDelta, jDelta) = (turnIndex[0], turnIndex[1], turnIndex[2], turnIndex[3]);

        figureTransform = _figureTransforms[i, j];

        startPosition = figureTransform.position;
        endPosition = IndexesToPosition(i + iDelta, j + jDelta);
        float distance = Vector3.Distance(startPosition, endPosition);
        float moveDuration = distance / _moveSpeed;

        await MoveFigure(startPosition, endPosition, moveDuration);

        _figureTransforms[i + iDelta, j + jDelta] = figureTransform;
        _figureTransforms[i, j] = null;
    }

    public async UniTask ChopFigure(int rivalI, int rivalJ)
    {
        Vector3 rivalPosition = IndexesToPosition(rivalI, rivalJ);
        float distance = Vector3.Distance(startPosition, rivalPosition);
        float removeDuration = distance / _moveSpeed;
        await RemoveFigure(_figureTransforms[rivalI, rivalJ], removeDuration);
    }

    public async UniTask MoveFigure(Vector3 startPosition, Vector3 endPosition, float moveDuration)
    {
        int index = Random.Range(0, _dragClips.Length);
        _audioSource.clip = _dragClips[index];
        _audioSource.Play();

        float elapsedTime = 0f;
        float t;

        while (elapsedTime < moveDuration)
        {
            t = elapsedTime / moveDuration;
            figureTransform.position = Vector3.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;

            await UniTask.Yield();
        }

        figureTransform.position = endPosition;
    }

    public async UniTask RemoveFigure(Transform figureTransform, float duration)
    {
        await UniTask.WaitForSeconds(duration);

        Destroy(figureTransform.gameObject);
    }

    public async UniTask CreateDam()
    {
        Transform childFigureTransform = figureTransform.GetChild(0);
        Vector3 figurePosition = figureTransform.position;
        Renderer renderer = childFigureTransform.GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;
        Vector3 crownPosition = figurePosition + new Vector3(0, bounds.center.y * 0.6f + bounds.extents.y, 0);

        var instantiateTask = InstantiateAsync(_crownPrefab, crownPosition, Quaternion.Euler(-90, 0, 0));
        await instantiateTask;
        GameObject crown = instantiateTask.Result[0];
        crown.transform.parent = childFigureTransform;
    }

    public void PlayEndSound(int winnerTurn)
    {
        _audioSource.clip = winnerTurn == 1 ? _winClip : _lossClip;
        _audioSource.Play();
    }

    public async UniTask PlayFigureAnimation(int i, int j, float startDelay)
    {
        await UniTask.WaitForSeconds(startDelay);

        Transform figureTransform = _figureTransforms[i, j];

        if (figureTransform != null)
        {
            Vector3 figurePosition = figureTransform.position;

            float expiredTime = 0;

            while (expiredTime < _jumpDuration)
            {
                float progress = expiredTime / _jumpDuration;
                float currentY = _jumpHeigh * _jumpCurve.Evaluate(progress);
                figureTransform.position = new Vector3(figurePosition.x, currentY, figurePosition.z);
                expiredTime += Time.deltaTime;

                await UniTask.Yield();
            }
            figureTransform.position = figurePosition;
        }
    }
}
