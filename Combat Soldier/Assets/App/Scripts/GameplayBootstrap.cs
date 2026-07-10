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

    private void Awake()
    {
        _poolConfigurator.InitializeManager();
        _repositoryManager.InitializeManager();

        Debug.Log("All managers were successfully initialized!");
    }

    private void Start()
    {
        _repositoryManager.InitializeAllTroops();
    }
}