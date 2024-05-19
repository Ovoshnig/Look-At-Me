using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private GameObject _musicPlayer;

    public override void InstallBindings()
    {
        BindMusic();
        BindSettings();
    }

    private void BindMusic()
    {
        var musicInstance = Container.InstantiatePrefabForComponent<MusicPlayer>(_musicPlayer);
        var audioSource = musicInstance.GetComponent<AudioSource>();
        Container.Bind<MusicPlayer>().FromInstance(musicInstance).AsSingle().NonLazy();
        Container.Bind<AudioSource>().FromInstance(audioSource).AsSingle().NonLazy();
    }

    private void BindSettings()
    {
        Container.BindInterfacesAndSelfTo<LevelSwitch>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LookSettings>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<AudioSettings>().FromNew().AsSingle().NonLazy();
    }
}
