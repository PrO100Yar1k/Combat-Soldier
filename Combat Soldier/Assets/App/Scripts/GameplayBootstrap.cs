using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly IEnemyTroopProvider _troopModelManager;
    private readonly IEnemyFactory _enemyFactoryManager;
    private readonly ITrenchFactory _trenchController;
    private readonly IObjectPool _poolConfigurator;

    private readonly RepositoryManager _repositoryManager;

    public GameplayBootstrap(RepositoryManager repositoryManager, IObjectPool poolConfigurator, IEnemyTroopProvider troopModelManager, IEnemyFactory enemyFactoryManager,
        ITrenchFactory trenchController)
    {
        _repositoryManager = repositoryManager;

        _poolConfigurator = poolConfigurator;
        _troopModelManager = troopModelManager;
        _enemyFactoryManager = enemyFactoryManager;
        _trenchController = trenchController;
    }

    public void Initialize()
    {
        _poolConfigurator.InitializePool();

        _repositoryManager.InitializeAllTroops();
        _repositoryManager.InitializeAllBuildings();

        _troopModelManager.ProvideEnemyVisionStarter();

        _trenchController.CreateTrench();
        _enemyFactoryManager.CreateEnemies();

        Debug.Log("Managers were succefully initialized!");
    }
}