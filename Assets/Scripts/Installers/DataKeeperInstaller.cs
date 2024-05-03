using Zenject;

public class DataKeeperInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ProgressKeeper>().AsSingle().NonLazy();
        Container.Bind<SensitivityKeeper>().AsSingle().NonLazy();
        Container.Bind<VolumeKeeper>().AsSingle().NonLazy();
    }
}
