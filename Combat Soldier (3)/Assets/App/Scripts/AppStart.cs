using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private TroopManager _troopManager = default;
    [SerializeField] private GameEvents _gameEvents = default;

    private void Awake()
    {
        _gameEvents.Initialize();

        _troopManager.InitializeManager();

        // to do
    }
}
