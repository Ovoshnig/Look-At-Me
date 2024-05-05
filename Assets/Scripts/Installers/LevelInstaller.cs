using Zenject;

public class LevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PauseMenuHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<FPSController>().FromComponentInHierarchy().AsSingle();
    }
}