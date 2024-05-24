using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindGameState();
        BindSettings();
    }

    private void BindGameState()
    {
        Container.BindInterfacesAndSelfTo<GameState>().FromNew().AsSingle().NonLazy();
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<LevelSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookSettings>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioSettings>().FromNew().AsSingle().NonLazy();
    }
}
