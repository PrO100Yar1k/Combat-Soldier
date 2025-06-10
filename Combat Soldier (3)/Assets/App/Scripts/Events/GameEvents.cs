using System;
using UnityEngine;

public class GameEvents : MonoBehaviour, IInitializeManager
{
    #region Singleton Activation

    [HideInInspector] public static GameEvents instance;

    public void InitializeManager()
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


    public event Action<TroopController, OrderMode> OnTroopEnterAnyMode = default;
    public void TroopEnterAnyMode(TroopController troopController, OrderMode orderMode) => OnTroopEnterAnyMode?.Invoke(troopController, orderMode);

    public event Action OnTroopCancelEnteringMode = default;
    public void TroopCancelEnteringMode() => OnTroopCancelEnteringMode?.Invoke();

    public event Action OnTroopDisableCanvases = default;
    public void TroopDisableCanvases() => OnTroopDisableCanvases?.Invoke();


    public event Action OnTroopStartedMovement = default;
    public void TroopStartedMovement() => OnTroopStartedMovement?.Invoke();

    public event Action OnTroopFinishedMovement = default;
    public void TroopFinishedMovement() => OnTroopFinishedMovement?.Invoke();
}
