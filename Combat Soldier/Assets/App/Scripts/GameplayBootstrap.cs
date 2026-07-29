using Assets.App.Scripts;
using Assets.App.Scripts.Core.Audio;
using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Managers;
using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly IEnemyTroopProvider _troopModelManager;
    private readonly IEnemyFactory _enemyFactoryManager;
    //private readonly ITrenchFactory _trenchController;
    private readonly IObjectPool _poolConfigurator;

    private readonly TroopRepository _troopRepository;
    private readonly AudioController _audioController;
    private readonly BuildingRepository _buildingRepository;
    private readonly BuildingTargetManager _buildingTargetManager;

    public GameplayBootstrap(TroopRepository troopRepository, BuildingRepository buildingRepository, AudioController audioController, BuildingTargetManager buildingTargetManager,
        IObjectPool poolConfigurator, IEnemyTroopProvider troopModelManager, IEnemyFactory enemyFactoryManager)
    {
        _troopRepository = troopRepository;
        _audioController = audioController;
        _buildingRepository = buildingRepository;

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

        //_trenchController.CreateTrench();
        _enemyFactoryManager.CreateEnemies();

        _buildingTargetManager.Initialize();

        _troopModelManager.StartEnemyModelVision();

        Debug.Log("Managers were succefully initialized!");
    }
}