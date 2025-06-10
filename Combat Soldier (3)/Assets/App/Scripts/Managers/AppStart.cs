using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private TroopGeneralManager _troopGeneralManager = default;
    [SerializeField] private PlayerTroopManager _playerTroopManager = default;

    [Space(2)]

    [SerializeField] private EnemyFactory _enemyFactory = default;
    [Space(2)]

    [SerializeField] private GameEvents _gameEvents = default;

    private void Awake()
    {
        _gameEvents.Initialize();

        _playerTroopManager.InitializeManager(); 
        _troopGeneralManager.InitializeManager();

        _enemyFactory.InitializeManager();

        // create interface for every of those IInitializeManager
        // controll right sequence of manager initializations
    }
}
