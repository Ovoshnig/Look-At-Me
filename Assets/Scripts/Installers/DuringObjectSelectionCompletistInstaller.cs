using Zenject;

public class DuringObjectSelectionCompletistInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<DuringObjectSelectionCompletist>().FromComponentInHierarchy().AsSingle();
    }
}
