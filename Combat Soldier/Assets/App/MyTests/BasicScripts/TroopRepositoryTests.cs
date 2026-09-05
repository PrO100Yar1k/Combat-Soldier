using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Repositories;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TroopRepositoryTests
{
    private GameEventBus _gameEventBus;
    private TroopRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _gameEventBus = new GameEventBus();
        _repository = new TroopRepository(_gameEventBus);
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
    }

    [Test]
    public void OnTroopSpawned_AddAllyTroop_ShouldContainTroopInPlayerList()
    {
        var troopObject = new GameObject("TestAllyTroop");
        var troopController = troopObject.AddComponent<TroopController>();

        _gameEventBus.TroopSpawned(troopController, Faction.Allies);

        var playerTroops = _repository.GetPlayerTroops();
        Assert.AreEqual(1, playerTroops.Count);
        Assert.Contains(troopController, playerTroops as System.Collections.ICollection);

        Object.DestroyImmediate(troopObject);
    }

    [Test]
    public void OnTroopDied_RemoveEnemyTroop_ShouldRemoveTroopFromEnemyList()
    {
        var troopObject = new GameObject("TestEnemyTroop");
        var troopController = troopObject.AddComponent<TroopController>();

        _gameEventBus.TroopSpawned(troopController, Faction.Enemies);
        Assert.AreEqual(1, _repository.GetEnemyTroops().Count);

        _gameEventBus.TroopDied(troopController, Faction.Enemies);

        Assert.AreEqual(0, _repository.GetEnemyTroops().Count);

        Object.DestroyImmediate(troopObject);
    }
}