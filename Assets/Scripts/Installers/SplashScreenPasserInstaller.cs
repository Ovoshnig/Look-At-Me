using Zenject;

public class SplashScreenPasserInstaller : MonoInstaller
{
    public override void InstallBindings() => Container.BindInterfacesAndSelfTo<SplashScreenPasser>().FromNew().AsSingle().NonLazy();
}
