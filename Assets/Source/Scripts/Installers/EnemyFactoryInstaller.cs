using Zenject;

public class EnemyFactoryInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EnemiesSpawner>().AsSingle().NonLazy();
        Container.BindFactory<Enemy, Enemy.Factory>();
    }
}