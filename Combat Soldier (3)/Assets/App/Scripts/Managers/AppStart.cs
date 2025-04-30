using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private TroopGeneralManager _troopGeneralManager = default;
    [SerializeField] private TroopManager _troopManager = default;

    [Space(2)]

    [SerializeField] private GameEvents _gameEvents = default;

    private void Awake()
    {
        _gameEvents.Initialize();

        _troopManager.InitializeManager();
        _troopGeneralManager.InitializeManager();
    }
}
