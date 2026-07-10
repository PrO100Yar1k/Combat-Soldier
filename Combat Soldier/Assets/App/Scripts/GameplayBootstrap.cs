using UnityEngine;
using Zenject;

public class GameplayBootstrap : MonoBehaviour
{
    private RepositoryManager _repositoryManager;
    private ObjectPoolConfigurator _poolConfigurator;

    [Inject]
    public void Construct(RepositoryManager repositoryManager, ObjectPoolConfigurator poolConfigurator)
    {
        _repositoryManager = repositoryManager;
        _poolConfigurator = poolConfigurator;
    }

    private void Start()
    {
        _poolConfigurator.InitializeBulletPool();

        _repositoryManager.InitializeAllTroops();
        _repositoryManager.InitializeAllBuildings();
    }
}