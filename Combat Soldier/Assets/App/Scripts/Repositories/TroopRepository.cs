using System;
using System.Linq;
using System.Collections.Generic;

namespace Assets.App.Scripts.Repositories
{
    public class TroopRepository : IDisposable
    {
        private readonly Dictionary<Faction, List<TroopController>> _troopsBySide = new()
        {
            { Faction.None, new List<TroopController>() },
            { Faction.Allies, new List<TroopController>() },
            { Faction.Enemies, new List<TroopController>() }
        };

        private readonly GameEventBus _gameEventBus;

        public TroopRepository(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;

            _gameEventBus.OnTroopSpawned += AddTroop;
            _gameEventBus.OnTroopDied += RemoveTroop;
        }

        public void Dispose()
        {
            _gameEventBus.OnTroopSpawned -= AddTroop;
            _gameEventBus.OnTroopDied -= RemoveTroop;
        }

        public void InitializeAll()
        {
            foreach (var controller in _troopsBySide.Values.SelectMany(list => list))
                controller.InitializeTroop();
        }

        public IReadOnlyList<TroopController> GetTroops(Faction faction) => _troopsBySide[faction];
        public IReadOnlyList<TroopController> GetPlayerTroops() => _troopsBySide[Faction.Allies];
        public IReadOnlyList<TroopController> GetEnemyTroops() => _troopsBySide[Faction.Enemies];

        private void AddTroop(TroopController troop, Faction faction) => _troopsBySide[faction].Add(troop);
        private void RemoveTroop(TroopController troop, Faction faction) => _troopsBySide[faction].Remove(troop);
    }
}
