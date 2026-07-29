using Assets.App.Scripts.Repositories;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class TargetSearchServiceTests
{
    private GameEventBus _gameEventBus;
    private TroopRepository _troopRepository;
    private BuildingRepository _buildingRepository;
    private TargetSearchService _searchService;

    private GameObject _closestEnemyObj;
    private GameObject _farEnemyObj;

    [SetUp]
    public void SetUp()
    {
        _gameEventBus = new GameEventBus();
        _troopRepository = new TroopRepository(_gameEventBus);
        _buildingRepository = new BuildingRepository(_gameEventBus);

        _searchService = new TargetSearchService(_troopRepository, _buildingRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _troopRepository.Dispose();
        _buildingRepository.Dispose();

        if (_closestEnemyObj != null) Object.DestroyImmediate(_closestEnemyObj);
        if (_farEnemyObj != null) Object.DestroyImmediate(_farEnemyObj);
    }

    [Test]
    public void GetClosestEnemyInRange_ShouldReturnClosestEnemy()
    {
        Vector3 origin = Vector3.zero;

        _closestEnemyObj = new GameObject("ClosestEnemy");
        _closestEnemyObj.transform.position = new Vector3(3f, 0f, 0f);
        var closestTroop = _closestEnemyObj.AddComponent<TestableTroopController>();

        _farEnemyObj = new GameObject("FarEnemy");
        _farEnemyObj.transform.position = new Vector3(8f, 0f, 0f);
        var farTroop = _farEnemyObj.AddComponent<TestableTroopController>();

        _gameEventBus.TroopSpawned(closestTroop, Faction.Enemies);
        _gameEventBus.TroopSpawned(farTroop, Faction.Enemies);

        float attackRange = 10f;
        var result = _searchService.GetClosestEnemyInRange(origin, attackRange, Faction.Enemies);

        Assert.IsNotNull(result, "Результат null, бо юніти не були знайдені в радіусі.");
        Assert.AreEqual(_closestEnemyObj, result.gameObject);
    }

    [Test]
    public void GetClosestEnemyInRange_WhenEnemyOutOfRange_ShouldReturnNull()
    {
        _farEnemyObj = new GameObject("FarEnemy");
        _farEnemyObj.transform.position = new Vector3(15f, 0f, 0f);
        var farTroop = _farEnemyObj.AddComponent<TestableTroopController>();

        _gameEventBus.TroopSpawned(farTroop, Faction.Enemies);

        float attackRange = 5f;
        var result = _searchService.GetClosestEnemyInRange(Vector3.zero, attackRange, Faction.Enemies);

        Assert.IsNull(result);
    }

    [Test]
    public void GetClosestEnemyInRange_WithPriorityTarget_ShouldReturnPriorityTarget()
    {
        Vector3 origin = Vector3.zero;

        var normalObj = new GameObject("NormalEnemy");
        normalObj.transform.position = new Vector3(2f, 0f, 0f);
        var normalTroop = normalObj.AddComponent<TestableTroopController>();

        var priorityObj = new GameObject("PriorityEnemy");
        priorityObj.transform.position = new Vector3(5f, 0f, 0f);
        var priorityTroop = priorityObj.AddComponent<TestableTroopController>();

        _gameEventBus.TroopSpawned(normalTroop, Faction.Enemies);
        _gameEventBus.TroopSpawned(priorityTroop, Faction.Enemies);

        var result = _searchService.GetClosestEnemyInRange(
            origin,
            range: 10f,
            enemyFaction: Faction.Enemies,
            priorityTarget: priorityTroop
        );

        Assert.AreEqual(priorityObj, result.gameObject);

        Object.DestroyImmediate(normalObj);
        Object.DestroyImmediate(priorityObj);
    }

    private class TestableTroopController : TroopController
    {
        protected override void OnEnable() { }
        protected override void OnDisable() { }

        public override void InitializeTroop() { }

        public new Faction GetFaction() => Faction.Enemies;
    }
}