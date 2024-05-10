using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private Settings _settings;

    public override void InstallBindings()
    {
        BindSettings();
        BindKeepers();
        BindLevelSwitch();
    }

    private void BindKeepers()
    {
        Container.BindInterfacesAndSelfTo<AchievedLevelKeeper>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SensitivityKeeper>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<VolumeKeeper>().AsSingle().NonLazy();
    }

    private void BindLevelSwitch()
    {
        Container.BindInterfacesAndSelfTo<LevelSwitch>().AsSingle().NonLazy();
    }

    private void BindSettings()
    {
        Container.BindInstance(_settings).AsSingle().NonLazy();
    }
}
