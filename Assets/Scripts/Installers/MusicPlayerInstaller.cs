using UnityEngine;
using Zenject;

[RequireComponent(typeof(MusicPlayer),
                  typeof(AudioSource))]
public class MusicPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var musicPlayer = GetComponent<MusicPlayer>();
        var audioSource = GetComponent<AudioSource>();
        Container.Bind<MusicPlayer>().FromInstance(musicPlayer).AsSingle().NonLazy();
        Container.Bind<AudioSource>().FromInstance(audioSource).AsSingle().NonLazy();
    }
}
