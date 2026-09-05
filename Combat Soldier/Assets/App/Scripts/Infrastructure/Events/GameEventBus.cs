using System;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Infrastructure.Events
{
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

        public event Action OnDeselectController = default;
        public void DeselectController() => OnDeselectController?.Invoke();

        public event Action OnDisableActiveCanvases = default;
        public void DisableActiveCanvas() => OnDisableActiveCanvases?.Invoke();

        public event Action<MonoBehaviour> OnOpenTroopMenu = default;
        public void OpenTroopMenu(MonoBehaviour controller) => OnOpenTroopMenu?.Invoke(controller);


        public event Action<ITrenchFactory> OnTrenchSpawned = default;
        public void TrenchSpawned(ITrenchFactory trenchController) => OnTrenchSpawned?.Invoke(trenchController);
    }
}
