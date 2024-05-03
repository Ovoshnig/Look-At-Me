using Zenject;

public class PauseMenuHandlerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PauseMenuHandler>().FromComponentInHierarchy().AsSingle();
    }
}