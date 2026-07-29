using Assets.App.Scripts;
using Assets.App.Scripts.Core.Audio;
using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private List<Transform> _enemyPatrollingPoints = new List<Transform>();

    [SerializeField, Space(3)] private BulletPoolConfigurator _poolConfigurator = default;
    [SerializeField] private PlayerSelectionController _troopSelectionController = default;
    [SerializeField] private PlayerCommandController _troopCommandController = default;

    [SerializeField] private AudioController _audioControllerPrefab = default;

    public override void InstallBindings()
    {
        Container.Bind<ICoroutineRunner>().To<CoroutineStarter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        Container.Bind<AudioController>().FromComponentInNewPrefab(_audioControllerPrefab).AsSingle().NonLazy();

        Container.Bind<List<Transform>>().WithId("Enemy Points").FromInstance(_enemyPatrollingPoints).AsSingle();

        Container.BindInterfacesTo<EnemyModelManager>().AsSingle();
        Container.BindInterfacesTo<EnemyFactoryManager>().AsSingle();

        //Container.Bind<ITroopSelection>().To<PlayerSelectionController>().FromInstance(_troopActionController).AsSingle();
        Container.Bind<PlayerSelectionController>().FromInstance(_troopSelectionController).AsSingle();
        Container.Bind<PlayerCommandController>().FromInstance(_troopCommandController).AsSingle();


        Container.Bind<IObjectPool>().To<BulletPoolConfigurator>().FromInstance(_poolConfigurator).AsSingle();

        Container.Bind<GameEventBus>().AsSingle();

        Container.BindInterfacesAndSelfTo<TroopRepository>().AsSingle();
        Container.BindInterfacesAndSelfTo<BuildingRepository>().AsSingle();

        Container.Bind<TargetSearchService>().AsSingle();
        Container.Bind<PatrolPointProvider>().AsSingle();
        Container.Bind<BuildingTargetManager>().AsSingle();

        Container.BindInterfacesTo<GameplayBootstrap>().AsSingle();
    }
}