using System;
using System.Collections.Generic;

namespace Assets.App.Scripts.Managers
{
    public class BuildingRepository : IDisposable
    {
        private readonly List<BuildingController> _enemyBuildings = new();
        private readonly GameEventBus _gameEventBus;

        public BuildingRepository(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
            _gameEventBus.OnBuildingSpawned += AddBuilding;
            _gameEventBus.OnBuildingDestroyed += RemoveBuilding;
        }

        public void Dispose()
        {
            _gameEventBus.OnBuildingSpawned -= AddBuilding;
            _gameEventBus.OnBuildingDestroyed -= RemoveBuilding;
        }

        public void InitializeAll()
        {
            foreach (var controller in _enemyBuildings)
                controller.InitializeBuilding();
        }

        public IReadOnlyList<BuildingController> GetEnemyBuildings() => _enemyBuildings;

        private void AddBuilding(BuildingController building) => _enemyBuildings.Add(building);
        private void RemoveBuilding(BuildingController building) => _enemyBuildings.Remove(building);
    }
}
