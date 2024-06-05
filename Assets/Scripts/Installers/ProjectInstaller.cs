using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioMixerGroup _audioMixerGroup;

    public override void InstallBindings()
    {
        BindGameState();
        BindDataSaver();
        BindAudioMixer();
        BindSettings();
    }

    private void BindGameState()
    {
        Container.BindInterfacesAndSelfTo<GameState>().FromNew().AsSingle().NonLazy();
    }

    private void BindDataSaver()
    {
        Container.BindInterfacesAndSelfTo<DataSaver>().FromNew().AsSingle().NonLazy();
    }

    private void BindAudioMixer()
    {
        Container.Bind<AudioMixerGroup>().FromInstance(_audioMixerGroup).AsSingle().NonLazy();
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<LevelSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookSettings>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioSettings>().FromNew().AsSingle().NonLazy();
    }
}
