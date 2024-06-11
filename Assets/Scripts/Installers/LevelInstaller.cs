using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private Button _resumeButton;

    public override void InstallBindings()
    {
        //Container.DeclareSignal<ResumeSignal>();
        //Container.BindSignal<ResumeSignal>().ToMethod<GameState>(x => x.Unpause).FromResolve();
        Container.Bind<PauseMenuHandler>().FromComponentInHierarchy().AsSingle();
    }   
}
