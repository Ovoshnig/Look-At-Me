using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SceneTransitionAnimator : MonoBehaviour
{
    [SerializeField] private Image _panelImage;

    private const float FullOpacityValue = 1f;
    private const float ZeroOpacityValue = 0f;

    private GameSettingsInstaller.LevelSettings _levelSettings;
    private CancellationTokenSource _cts = new();

    [Inject]
    private void Construct(GameSettingsInstaller.LevelSettings levelSettings) => _levelSettings = levelSettings;

    private void Start() => LaunchAnimation().Forget();

    private void OnDisable()
    {
        if ( _cts != null )
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }

    private async UniTask LaunchAnimation()
    {
        float startOpacity = ZeroOpacityValue;
        float endOpacity = FullOpacityValue;
        await PlayAnimation(startOpacity, endOpacity);

        startOpacity = FullOpacityValue;
        endOpacity = ZeroOpacityValue;
        await PlayAnimation(startOpacity, endOpacity);
    }

    private async UniTask PlayAnimation(float startOpacity, float endOpacity)
    {
        CancellationToken token = _cts.Token;
        float duration = _levelSettings.LevelTransitionDuration / 2;
        float elapsedTime = 0f;
        float t;

        while (elapsedTime < duration)
        {
            t = elapsedTime / duration;
            float currentValue = Mathf.Lerp(startOpacity, endOpacity, t);
            Color color = _panelImage.color;
            color.a = currentValue;
            _panelImage.color = color;

            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);
        }
    }
}
