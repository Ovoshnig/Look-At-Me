using Zenject;

public class OneSoundAtTimeProviderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<OneSoundAtTimeProvider>().AsSingle();
    }
}
