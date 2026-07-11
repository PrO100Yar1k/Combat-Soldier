using UnityEngine;
using Zenject;

public class GameplayBootstrap : IInitializable
{
    private readonly RepositoryManager _repositoryManager;
    private readonly ObjectPoolConfigurator _poolConfigurator;

    public GameplayBootstrap(RepositoryManager repositoryManager, ObjectPoolConfigurator poolConfigurator)
    {
        _repositoryManager = repositoryManager;
        _poolConfigurator = poolConfigurator;
    }

    public void Initialize()
    {
        _poolConfigurator.InitializeBulletPool();

        _repositoryManager.InitializeAllTroops();
        _repositoryManager.InitializeAllBuildings();

        Debug.Log("Managers were succefully initialized!");
    }
}