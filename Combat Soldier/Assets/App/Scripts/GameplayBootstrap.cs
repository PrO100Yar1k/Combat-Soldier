using Assets.App.Scripts;
using Assets.App.Scripts.Core.Audio;
using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Managers;
using Assets.App.Scripts.Repositories;
using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly IEnemyTroopProvider _troopModelManager;
    private readonly IEnemyFactory _enemyFactoryManager;
    private readonly IObjectPool _poolConfigurator;

    private readonly TroopRepository _troopRepository;
    private readonly BuildingRepository _buildingRepository;
    private readonly TrenchRepository _trenchRepository;

    private readonly AudioController _audioController;
    private readonly BuildingTargetManager _buildingTargetManager;

    public GameplayBootstrap(TroopRepository troopRepository, BuildingRepository buildingRepository, TrenchRepository trenchRepository, 
        AudioController audioController, BuildingTargetManager buildingTargetManager,
        IObjectPool poolConfigurator, IEnemyTroopProvider troopModelManager, IEnemyFactory enemyFactoryManager)
    {
        _audioController = audioController;

        _troopRepository = troopRepository;
        _buildingRepository = buildingRepository;
        _trenchRepository = trenchRepository;

        _poolConfigurator = poolConfigurator;

        _troopModelManager = troopModelManager;
        _enemyFactoryManager = enemyFactoryManager;
        _buildingTargetManager = buildingTargetManager;

    }

    public void Initialize()
    {
        _audioController.PlayBackgroundSoundtrack();

        _poolConfigurator.InitializePool();

        _troopRepository.InitializeAll();
        _buildingRepository.InitializeAll();
        _trenchRepository.InitializeAll();

        _enemyFactoryManager.CreateEnemies();

        _buildingTargetManager.Initialize();

        _troopModelManager.StartProvidingEnemyDeploymentVision();

        Debug.Log("Managers were succefully initialized!");
    }
}