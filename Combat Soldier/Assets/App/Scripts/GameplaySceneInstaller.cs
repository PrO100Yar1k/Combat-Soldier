using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplaySceneInstaller : MonoInstaller
{
    [SerializeField] private List<Transform> _enemyPatrollingPoints = new List<Transform>();
    [SerializeField] private ObjectPoolConfigurator _poolConfigurator = default;

    public override void InstallBindings()
    {
        Container.Bind<CoroutineStarter>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<List<Transform>>()
            .WithId("Enemy Points")
            .FromInstance(_enemyPatrollingPoints)
            .AsSingle();


        Container.Bind<TroopModelManager>().AsSingle();

        Container.Bind<RepositoryManager>().AsSingle();
        Container.Bind<GameEvents>().AsSingle();

        Container.Bind<ObjectPoolConfigurator>().FromInstance(_poolConfigurator).AsSingle();

        Container.Bind<GameplayBootstrap>().FromComponentInHierarchy().AsSingle();
    }
}