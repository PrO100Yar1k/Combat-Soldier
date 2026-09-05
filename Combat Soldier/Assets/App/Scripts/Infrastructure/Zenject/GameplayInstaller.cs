using System.Collections.Generic;
using App.Scripts.Core.Audio;
using App.Scripts.Core.ObjectPool;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.Model;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Infrastructure.Others;
using App.Scripts.Managers;
using App.Scripts.Repositories;
using UnityEngine;
using Zenject;

namespace App.Scripts.Infrastructure.Zenject
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private BulletPoolConfigurator _poolConfigurator;
        [SerializeField] private PlayerSelectionController _troopSelectionController;
        [SerializeField] private PlayerCommandController _troopCommandController;

        [SerializeField] private AudioController _audioControllerPrefab;

        [SerializeField, Space(3)] private List<Transform> _enemyPatrollingPoints = new();

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineRunner>().To<CoroutineStarter>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<AudioController>().FromComponentInNewPrefab(_audioControllerPrefab).AsSingle().NonLazy(); //

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
            Container.BindInterfacesAndSelfTo<TrenchRepository>().AsSingle();

            Container.Bind<TargetSearchService>().AsSingle();
            Container.Bind<PatrolPointProvider>().AsSingle();
            Container.Bind<BuildingTargetManager>().AsSingle();

            Container.BindInterfacesTo<GameplayBootstrap>().AsSingle();
        }
    }
}