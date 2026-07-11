using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private List<Transform> _enemyPatrollingPoints = new List<Transform>();
    [SerializeField, Space(3)] private ObjectPoolConfigurator _poolConfigurator = default;
    [SerializeField] private TroopActionController _troopActionController = default;
    [SerializeField] private LineTrenchController _trenchController = default;

    public override void InstallBindings()
    {
        Container.Bind<CoroutineStarter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<List<Transform>>()
            .WithId("Enemy Points")
            .FromInstance(_enemyPatrollingPoints)
            .AsSingle();

        Container.Bind<TroopModelManager>().AsSingle(); 
        Container.Bind<RepositoryManager>().AsSingle();
        Container.Bind<EnemyFactoryManager>().AsSingle(); 

        Container.Bind<GameEvents>().AsSingle();

        Container.Bind<TroopActionController>().FromInstance(_troopActionController).AsSingle();
        Container.Bind<ObjectPoolConfigurator>().FromInstance(_poolConfigurator).AsSingle();
        Container.Bind<LineTrenchController>().FromInstance(_trenchController).AsSingle();

        Container.BindInterfacesTo<GameplayBootstrap>().AsSingle();
    }
}