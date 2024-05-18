using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private Settings _settings;

    public override void InstallBindings()
    {
        BindSettings();
    }

    private void BindSettings()
    {
        Container.BindInstance(_settings).AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookSettings>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioSettings>().FromNew().AsSingle().NonLazy();
    }
}
