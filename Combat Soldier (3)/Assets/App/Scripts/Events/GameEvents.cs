using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton Activation

    [HideInInspector] public static GameEvents instance;

    public void Initialize()
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


    public event Action<Vector3, Action> OnTroopStartedMovement = default;
    public void TroopMovement(Vector3 point, Action finishAction) => OnTroopStartedMovement?.Invoke(point, finishAction);
    
    public event Action<TroopController> OnTroopStartedAttack = default;
    public void TroopStartedAttack(TroopController enemyController) => OnTroopStartedAttack?.Invoke(enemyController);


    public event Action<TroopController, OrderMode> OnTroopEnterAnyMode = default;
    public void TroopEnterAnyMode(TroopController troopController, OrderMode orderMode) => OnTroopEnterAnyMode?.Invoke(troopController, orderMode);

    public event Action OnTroopCancelEnteringMode = default;
    public void TroopCancelEnteringMode() => OnTroopCancelEnteringMode?.Invoke();

    public event Action OnDisableCanvases = default;
    public void DisableCanvases() => OnDisableCanvases?.Invoke();
}
