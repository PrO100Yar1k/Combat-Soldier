using Assets.App.Scripts.Repositories;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class BuildingRepositoryTests
{
    private GameEventBus _gameEventBus;
    private BuildingRepository _repository;

    private GameObject _buildingObj;

    [SetUp]
    public void SetUp()
    {
        _gameEventBus = new GameEventBus();
        _repository = new BuildingRepository(_gameEventBus);
    }

    [TearDown]
    public void TearDown()
    {
        _repository.Dispose();
        if (_buildingObj != null) Object.DestroyImmediate(_buildingObj);
    }

    [Test]
    public void OnBuildingSpawned_ShouldAddBuildingToRepository()
    {
        _buildingObj = new GameObject("TestBuilding");
        var building = _buildingObj.AddComponent<TestableBuildingController>();

        _gameEventBus.BuildingSpawned(building);

        Assert.AreEqual(1, _repository.GetEnemyBuildings().Count);
        Assert.Contains(building, _repository.GetEnemyBuildings() as System.Collections.ICollection);
    }

    [Test]
    public void OnBuildingDestroyed_ShouldRemoveBuildingFromRepository()
    {
        _buildingObj = new GameObject("TestBuilding");
        var building = _buildingObj.AddComponent<TestableBuildingController>();
        _gameEventBus.BuildingSpawned(building);

        _gameEventBus.BuildingDestroyed(building);

        Assert.AreEqual(0, _repository.GetEnemyBuildings().Count);
    }

    private class TestableBuildingController : BuildingController
    {
        public override void InitializeBuilding() { }
        protected override void InitializeBuildingBehaviour() { }

        protected override void OnEnable() { }
        protected override void OnDisable() { }
    }
}