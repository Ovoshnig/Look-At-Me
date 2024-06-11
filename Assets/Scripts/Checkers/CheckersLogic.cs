using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CheckersVisualizer))]
public class CheckersLogic : MonoBehaviour
{
    [SerializeField] private float _placementDelay;
    [SerializeField] private float _finishDelay;
    [SerializeField] private CheckersVisualizer _visualizer;

    private SceneSwitch _sceneSwitch;

    private readonly int[,] _board = new int[8, 8];
    private uint _player1Count = 12;
    private uint _player2Count = 12;
    private int _turn = 1;

    [Inject]
    private void Construct(SceneSwitch sceneSwitch) => _sceneSwitch = sceneSwitch;

    private void Awake() => _visualizer = GetComponent<CheckersVisualizer>();

    public async UniTaskVoid StartPlacement()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                _board[i, j] = 0;

        foreach (int delta in new int[] { 0, 5 })
        {
            int playerIndex = delta == 0 ? 0 : 1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = delta; j < 3 + delta; j++)
                {
                    if (i % 2 == j % 2)
                    {
                        _board[i, j] = delta == 0 ? 1 : 2;

                        _visualizer.PlaceFigure(i, j, playerIndex);
                    }

                    await UniTask.WaitForSeconds(_placementDelay);
                }
            }
        }

        EnumerateMoves().Forget();
    }

    private bool IsCanMove(int i, int j)
    {
        return i > -1 && i < 8 && 
               j > -1 && j < 8;
    }

    private bool IsRival(int i, int j)
    {
        int rivalFigure;
        int rivalDam;

        if (_turn == 1)
        {
            rivalFigure = 2;
            rivalDam = 4;
        }
        else
        {
            rivalFigure = 1;
            rivalDam = 3;
        }

        return (_board[i, j] == rivalFigure ||
                _board[i, j] == rivalDam);
    }

    private async UniTaskVoid EnumerateMoves()
    {
        int zForwardCoefficient = _turn == 1 ? 1 : -1;

        List<List<int>> chopIndexes = new();
        List<List<int>> moveIndexes = new();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (_board[i, j] == _turn) // Фигура текущего игрока
                {
                    int jDelta = zForwardCoefficient;

                    foreach (int iDelta in new int[] { -1, 1 }) // Проверки всех вариантов ходов вперёд
                    {
                        if (IsCanMove(i + iDelta, j + jDelta))
                        {
                            if (IsRival(i + iDelta, j + jDelta))
                            {
                                if (IsCanMove(i + 2 * iDelta, j + 2 * jDelta) &&
                                       _board[i + 2 * iDelta, j + 2 * jDelta] == 0)
                                {
                                    chopIndexes.Add(new List<int> { i, j, 2 * iDelta, 2 * jDelta, i + iDelta, j + jDelta });
                                }
                            }
                            else if (_board[i + iDelta, j + jDelta] == 0)
                            {
                                moveIndexes.Add(new List<int> { i, j, iDelta, jDelta });
                            }
                        }
                        
                        await UniTask.Yield();
                    }

                    jDelta = -jDelta;

                    foreach (int iDelta in new int[] { -1, 1 }) // Проверки, есть ли сзади противник, которого можно срубить
                    {
                        if (IsCanMove(i + iDelta, j + jDelta) &&
                              IsRival(i + iDelta, j + jDelta))
                        {
                            if (IsCanMove(i + 2 * iDelta, j + 2 * jDelta) &&
                                   _board[i + 2 * iDelta, j + 2 * jDelta] == 0)
                            {
                                chopIndexes.Add(new List<int> { i, j, 2 * iDelta, 2 * jDelta, i + iDelta, j + jDelta });
                            }
                        }

                        await UniTask.Yield();
                    }
                }
                else if (_board[i, j] == _turn + 2) // Дамка текущего игрока
                {
                    foreach (int iDelta in new int[] { -1, 1 })
                    {
                        foreach (int jDelta in new int[] { -1, 1 })
                        {
                            List<int> rivalIndexes = new();

                            int moveLength = 1;
                            int rivalCount = 0;

                            while (IsCanMove(i + moveLength * iDelta, j + moveLength * jDelta))
                            {
                                if (IsRival(i + moveLength * iDelta, j + moveLength * jDelta))
                                {
                                    if (rivalCount == 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        rivalIndexes.Add(i + moveLength * iDelta);
                                        rivalIndexes.Add(j + moveLength * jDelta);

                                        rivalCount++;
                                    }
                                }   
                                else if (_board[i + moveLength * iDelta, j + moveLength * jDelta] == 0)
                                {
                                    if (rivalCount == 1)
                                    {
                                        chopIndexes.Add(new List<int> { i, j, moveLength * iDelta, moveLength * jDelta, rivalIndexes[0], rivalIndexes[1] });
                                    }
                                    else
                                    {
                                        moveIndexes.Add(new List<int> { i, j, moveLength * iDelta, moveLength * jDelta });
                                    }
                                }
                                else
                                {
                                    break;
                                }

                                moveLength++;

                                await UniTask.Yield();
                            }
                        }
                    }
                }
            }
        }

        // Принятие решения хода
        if (chopIndexes.Count > 0)
        {
            SelectRandomMove(chopIndexes, true);
        }
        else if (moveIndexes.Count > 0)
        {
            SelectRandomMove(moveIndexes, false);
        }
        else
        {
            int winnerTurn = _turn == 1 ? 2 : 1;
            Win(winnerTurn).Forget();
        }
    }

    private async UniTask TryChop(int i, int j)
    {
        List<List<int>> chopIndexes = new();

        if (_board[i, j] == _turn) // Ходит фигура
        {
            foreach (int iDelta in new int[] { -1, 1 }) // Проверки, есть ли сзади противник, которого можно срубить
            {
                foreach (int jDelta in new int[] { -1, 1 })
                {

                    if (IsCanMove(i + iDelta, j + jDelta) &&
                      IsRival(i + iDelta, j + jDelta) &&
                    IsCanMove(i + 2 * iDelta, j + 2 * jDelta) &&
                       _board[i + 2 * iDelta, j + 2 * jDelta] == 0)
                    {
                        chopIndexes.Add(new List<int> { i, j, 2 * iDelta, 2 * jDelta, i + iDelta, j + jDelta });
                    }

                    await UniTask.Yield();
                }
            }
        }
        else if (_board[i, j] == _turn + 2) // Ходит дамка
        {
            foreach (int iDelta in new int[] { -1, 1 }) // Проверки, есть ли сзади противник, которого можно срубить
            {
                foreach (int jDelta in new int[] { -1, 1 })
                {
                    List<int> rivalIndexes = new();

                    int moveLength = 1;
                    int rivalCount = 0;

                    while (IsCanMove(i + moveLength * iDelta, j + moveLength * jDelta))
                    {
                        if (IsRival(i + moveLength * iDelta, j + moveLength * jDelta))
                        {
                            if (rivalCount == 1)
                            {
                                break;
                            }
                            else
                            {
                                rivalIndexes.Add(i + moveLength * iDelta);
                                rivalIndexes.Add(j + moveLength * jDelta);

                                rivalCount++;
                            }
                        }
                        else if (_board[i + moveLength * iDelta, j + moveLength * jDelta] == 0)
                        {
                            if (rivalCount == 1)
                            {
                                chopIndexes.Add(new List<int> { i, j, moveLength * iDelta, moveLength * jDelta, rivalIndexes[0], rivalIndexes[1] });
                            }
                        }
                        else
                        {
                            break;
                        }

                        moveLength++;

                        await UniTask.Yield();
                    }
                }
            }
        }

        // Принятие решения хода
        if (chopIndexes.Count > 0)
        {
            SelectRandomMove(chopIndexes, true);
        }
        else
        {
            _turn = _turn == 1 ? 2 : 1;
            EnumerateMoves().Forget();
        }
    }

    private void SelectRandomMove(List<List<int>> turnIndexes, bool isChoping = false)
    {
        int randomIndex = Random.Range(0, turnIndexes.Count);
        List<int> randomTurn = turnIndexes[randomIndex];

        if (isChoping)
            MakeChopMove(randomTurn).Forget();
        else
            MakeMove(randomTurn).Forget();
    }

    private async UniTaskVoid MakeMove(List<int> turnIndex)
    {
        var (i, j, iDelta, jDelta) = (turnIndex[0], turnIndex[1], turnIndex[2], turnIndex[3]);

        await _visualizer.MakeFigureMove(turnIndex);

        int oppositeBoardSide = _turn == 1 ? 7 : 0;

        if (_board[i, j] == _turn)
        {
            if (j + jDelta == oppositeBoardSide)
            {
                _board[i + iDelta, j + jDelta] = _turn + 2;

                await _visualizer.CreateDam();
            }
            else
            {
                _board[i + iDelta, j + jDelta] = _turn;
            }
        }
        else if (_board[i, j] == _turn + 2)
        {
            _board[i + iDelta, j + jDelta] = _turn + 2;
        }

        _board[i, j] = 0;

        _turn = _turn == 1 ? 2 : 1;
        EnumerateMoves().Forget();
    }

    private async UniTaskVoid MakeChopMove(List<int> turnIndex)
    {
        var (i, j, iDelta, jDelta, rivalI, rivalJ) = 
            (turnIndex[0], turnIndex[1], turnIndex[2], turnIndex[3], turnIndex[4], turnIndex[5]);

        UniTask movementTask = _visualizer.MakeFigureMove(turnIndex);

        _board[rivalI, rivalJ] = 0;
        await _visualizer.ChopFigure(rivalI, rivalJ);
        await movementTask;

        if (_turn == 1)
        {
            _player2Count--;
            if (_player2Count == 0)
            {
                Win(1).Forget();
                return;
            }
        }
        else
        {
            _player1Count--;
            if (_player1Count == 0)
            {
                Win(2).Forget();
                return;
            }
        }

        int oppositeBoardSide = _turn == 1 ? 7 : 0;

        if (_board[i, j] == _turn)
        {
            if (j + jDelta == oppositeBoardSide)
            {
                _board[i + iDelta, j + jDelta] = _turn + 2;

                await _visualizer.CreateDam();
            }
            else
            {
                _board[i + iDelta, j + jDelta] = _turn;
            }
        }
        else if (_board[i, j] == _turn + 2)
        {
            _board[i + iDelta, j + jDelta] = _turn + 2;
        }

        _board[i, j] = 0;

        TryChop(i + iDelta, j + jDelta).Forget();
    }

    private async UniTaskVoid Win(int winnerTurn)
    {
        _visualizer.PlayEndSound(winnerTurn);

        float startDelay;
        List<UniTask> tasks = new();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (_board[i, j] == winnerTurn)
                {
                    startDelay = Random.Range(0, _finishDelay);

                    UniTask task = _visualizer.PlayFigureAnimation(i, j, startDelay);
                    tasks.Add(task);
                }
            }
        }

        await UniTask.WhenAll(tasks);

        _sceneSwitch.TryLoadNextLevelFirstTime().Forget();
    }
}
