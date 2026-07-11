using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly TroopModelManager _troopModelManager;
    private readonly RepositoryManager _repositoryManager;
    private readonly EnemyFactoryManager _enemyFactoryManager;
    private readonly LineTrenchController _trenchController;
    private readonly ObjectPoolConfigurator _poolConfigurator;

    public GameplayBootstrap(RepositoryManager repositoryManager, ObjectPoolConfigurator poolConfigurator, TroopModelManager troopModelManager, EnemyFactoryManager enemyFactoryManager,
        LineTrenchController trenchController)
    {
        _repositoryManager = repositoryManager;
        _poolConfigurator = poolConfigurator;
        _troopModelManager = troopModelManager;
        _enemyFactoryManager = enemyFactoryManager;
        _trenchController = trenchController;
    }

    public void Initialize()
    {
        _poolConfigurator.InitializeBulletPool();

        _repositoryManager.InitializeAllTroops();
        _repositoryManager.InitializeAllBuildings();

        _troopModelManager.ProvideEnemyVisionStarter();
        _enemyFactoryManager.CreateEnemies();
        _trenchController.CreateTrench();

        Debug.Log("Managers were succefully initialized!");
    }
}