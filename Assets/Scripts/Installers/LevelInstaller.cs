using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private Button _resumeButton;

    public override void InstallBindings()
    {
        Container.Bind<PauseMenuHandler>().FromComponentInHierarchy().AsSingle();
    }   
}
