using Zenject;

public class ObjectsInCorrectStatesCounterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ObjectsInCorrectStatesCounter>().AsSingle();
    }
}
