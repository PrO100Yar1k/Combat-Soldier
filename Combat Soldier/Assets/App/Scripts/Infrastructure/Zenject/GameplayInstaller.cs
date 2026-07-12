using Assets.App.Scripts.Infrastructure.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private List<Transform> _enemyPatrollingPoints = new List<Transform>();

    [SerializeField, Space(3)] private BulletPoolConfigurator _poolConfigurator = default;
    [SerializeField] private TroopActionController _troopActionController = default;
    [SerializeField] private TrenchLineController _trenchController = default;

    public override void InstallBindings()
    {
        Container.Bind<CoroutineStarter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<List<Transform>>().WithId("Enemy Points").FromInstance(_enemyPatrollingPoints).AsSingle();

        Container.BindInterfacesTo<TroopModelManager>().AsSingle();
        Container.BindInterfacesTo<EnemyFactoryManager>().AsSingle();

        Container.BindInterfacesTo<TrenchLineController>().FromInstance(_trenchController).AsSingle();

        Container.Bind<ITroopSelection>().To<TroopActionController>().FromInstance(_troopActionController).AsSingle();
        Container.Bind<IObjectPool>().To<BulletPoolConfigurator>().FromInstance(_poolConfigurator).AsSingle();

        Container.Bind<RepositoryManager>().AsSingle();
        Container.Bind<GameEventBus>().AsSingle();

        Container.BindInterfacesTo<GameplayBootstrap>().AsSingle();
    }
}