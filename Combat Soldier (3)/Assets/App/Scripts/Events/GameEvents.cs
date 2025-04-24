using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton activation
    [HideInInspector] public static GameEvents instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public event Action<TroopController, TroopSide> OnTroopSpawned = default;
    public void TroopSpawned(TroopController troopController, TroopSide troopSide) => OnTroopSpawned?.Invoke(troopController, troopSide);

    public event Action<TroopController, TroopSide> OnTroopDied = default;
    public void TroopDied(TroopController troopController, TroopSide troopSide) => OnTroopDied?.Invoke(troopController, troopSide);


    public event Action<Vector3, float> OnTroopStartedMovement = default;
    public void TroopMovement(Vector3 point, float speed) => OnTroopStartedMovement?.Invoke(point, speed);


    public event Action<TroopController, OrderMode> OnTroopEnterAnyMode = default;
    public void TroopEnterAnyMode(TroopController troopController, OrderMode orderMode) => OnTroopEnterAnyMode?.Invoke(troopController, orderMode);

    public event Action OnTroopCancelEnteringMode = default;
    public void TroopCancelEnteringMode() => OnTroopCancelEnteringMode?.Invoke();
}
