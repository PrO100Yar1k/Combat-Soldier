using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Managers;
using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly IEnemyTroopProvider _troopModelManager;
    private readonly IEnemyFactory _enemyFactoryManager;
    //private readonly ITrenchFactory _trenchController;
    private readonly IObjectPool _poolConfigurator;

    private readonly TroopRepository _troopRepository;
    private readonly BuildingRepository _buildingRepository;

    public GameplayBootstrap(TroopRepository troopRepository, BuildingRepository buildingRepository, IObjectPool poolConfigurator, IEnemyTroopProvider troopModelManager, IEnemyFactory enemyFactoryManager)
    {
        _troopRepository = troopRepository;
        _buildingRepository = buildingRepository;

        _poolConfigurator = poolConfigurator;
        _troopModelManager = troopModelManager;
        _enemyFactoryManager = enemyFactoryManager;
    }

    public void Initialize()
    {
        _poolConfigurator.InitializePool();

        _troopRepository.InitializeAll();
        _buildingRepository.InitializeAll();

        _troopModelManager.ProvideEnemyVisionStarter();

        //_trenchController.CreateTrench();
        _enemyFactoryManager.CreateEnemies();

        Debug.Log("Managers were succefully initialized!");
    }
}