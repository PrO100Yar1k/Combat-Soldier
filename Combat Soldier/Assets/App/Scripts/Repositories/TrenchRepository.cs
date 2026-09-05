using System;
using System.Collections.Generic;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Repositories
{
    public class TrenchRepository : IDisposable
    {
        private readonly List<ITrenchFactory> _trenchList = new();
        private readonly GameEventBus _gameEventBus;

        public TrenchRepository(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
            _gameEventBus.OnTrenchSpawned += AddTrench;
        }

        public void Dispose()
        {
            _gameEventBus.OnTrenchSpawned -= RemoveTrench;
        }

        public void InitializeAll()
        {
            foreach (var controller in _trenchList)
                controller.CreateTrench();
        }

        public IReadOnlyList<ITrenchFactory> GetTrenchController() => _trenchList;

        private void AddTrench(ITrenchFactory trenchController) => _trenchList.Add(trenchController);
        private void RemoveTrench(ITrenchFactory trenchController) => _trenchList.Remove(trenchController);
    }
}
