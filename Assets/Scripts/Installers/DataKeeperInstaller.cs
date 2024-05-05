using Zenject;

public class DataKeeperInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AchievedLevelKeeper>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SensitivityKeeper>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<VolumeKeeper>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelSwitch>().AsSingle().NonLazy();
    }
}
