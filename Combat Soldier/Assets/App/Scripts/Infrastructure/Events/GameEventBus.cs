using Assets.App.Scripts;
using System;
using UnityEngine;

public class GameEventBus
{
    public event Action<TroopController, Faction> OnTroopSpawned = default;
    public void TroopSpawned(TroopController troopController, Faction troopSide) => OnTroopSpawned?.Invoke(troopController, troopSide);

    public event Action<TroopController, Faction> OnTroopDied = default;
    public event Action<TroopController> OnTroopDiedUI = default;

    public void TroopDied(TroopController troopController, Faction troopSide)
    {
        OnTroopDied?.Invoke(troopController, troopSide);
        OnTroopDiedUI?.Invoke(troopController);
    }

    public event Action<TroopController> OnTroopDisableUI = default;
    public void TroopDisableUI(TroopController troopController) => OnTroopDisableUI?.Invoke(troopController);


    public event Action<BuildingController> OnBuildingSpawned = default;
    public void BuildingSpawned(BuildingController buildingController) => OnBuildingSpawned?.Invoke(buildingController);

    public event Action<BuildingController> OnBuildingDestroyed = default;
    public void BuildingDestroyed(BuildingController buildingController) => OnBuildingDestroyed?.Invoke(buildingController);


    public event Action<TroopController, OrderMode> OnTroopEnterAnyMode = default;
    public void TroopEnterAnyMode(TroopController troopController, OrderMode orderMode) => OnTroopEnterAnyMode?.Invoke(troopController, orderMode);

    public event Action OnTroopCancelEnteringMode = default;
    public void TroopCancelEnteringMode() => OnTroopCancelEnteringMode?.Invoke();


    public event Action OnDisableActiveCanvases = default;
    public void DisableActiveCanvases() => OnDisableActiveCanvases?.Invoke();

    public event Action<MonoBehaviour> OnOpenTroopMenu = default;
    public void OpenTroopMenu(MonoBehaviour controller) => OnOpenTroopMenu?.Invoke(controller);
}
