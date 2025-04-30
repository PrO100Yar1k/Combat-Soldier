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

    public event Action<TroopController, Vector3, Action> OnTroopMoveToPoint = default;
    public void TroopMoveToPoint(TroopController troopController, Vector3 point, Action finishAction)
    {
        finishAction += OnTroopFinishedMovement;

        OnTroopStartedMovement?.Invoke();
        OnTroopMoveToPoint?.Invoke(troopController, point, finishAction);
    }
    
    public event Action<TroopController> OnTroopAttackEnemy = default;
    public void TroopAttackEnemy(TroopController enemyController) => OnTroopAttackEnemy?.Invoke(enemyController);


    public event Action<TroopController, OrderMode> OnTroopEnterAnyMode = default;
    public void TroopEnterAnyMode(TroopController troopController, OrderMode orderMode) => OnTroopEnterAnyMode?.Invoke(troopController, orderMode);

    public event Action OnTroopCancelEnteringMode = default;
    public void TroopCancelEnteringMode() => OnTroopCancelEnteringMode?.Invoke();

    public event Action OnTroopDisableCanvases = default;
    public void TroopDisableCanvases() => OnTroopDisableCanvases?.Invoke();

    public event Action OnTroopStartedMovement = default;
    public event Action OnTroopFinishedMovement = default;
}
