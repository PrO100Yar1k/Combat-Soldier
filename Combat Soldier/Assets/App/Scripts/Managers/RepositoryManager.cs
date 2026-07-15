using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class RepositoryManager : System.IDisposable // to do
{
    private readonly Dictionary<Faction, List<TroopController>> _troopsBySide = new()
    {
        { Faction.None, new List<TroopController>() },
        { Faction.Allies, new List<TroopController>() },
        { Faction.Enemies, new List<TroopController>() }
    };
    private readonly List<BuildingController> _buildingControllersEnemyList = new List<BuildingController>();

    private readonly GameEventBus _gameEventBus;
    private readonly List<Transform> _enemyPatrollingPoins;

    #region Events & Interfaces

    public void Dispose()
    {
        UnSubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        _gameEventBus.OnTroopSpawned += AddTroopToList;
        _gameEventBus.OnTroopDied += RemoveTroopFromList;

        _gameEventBus.OnBuildingSpawned += AddBuildingToList;
        _gameEventBus.OnBuildingDestroyed += RemoveBuildingFromList;
    }

    private void UnSubscribeFromEvents()
    {
        _gameEventBus.OnTroopSpawned -= AddTroopToList;
        _gameEventBus.OnTroopDied -= RemoveTroopFromList;

        _gameEventBus.OnBuildingSpawned -= AddBuildingToList;
        _gameEventBus.OnBuildingDestroyed -= RemoveBuildingFromList;
    }

    #endregion

    public RepositoryManager(GameEventBus gameEvents, [Inject(Id = "Enemy Points")] List<Transform> enemyPatrollingPoints)
    {
        _gameEventBus = gameEvents;
        _enemyPatrollingPoins = new List<Transform>(enemyPatrollingPoints);

        SubscribeToEvents();
    }

    public void InitializeAllTroops()
    {
        var allTroops = _troopsBySide.Values.SelectMany(list => list);

        foreach (TroopController controller in allTroops)
            controller.InitializeTroop();
    }

    public void InitializeAllBuildings()
    {
        foreach (BuildingController controller in _buildingControllersEnemyList)
            controller.InitializeBuilding();
    }

    // make extension methods
    public TroopController[] GetEnemyListInRange(Vector3 troopPosition, float troopRange, Faction enemyTroopSide)
    {
        return GetTroopControllersList(enemyTroopSide)
            .Where(troop => Vector3.Distance(troopPosition, troop.transform.position) <= troopRange)
            .ToArray();
    }

    public MonoBehaviour GetClosestEnemyInRange(Vector3 troopPosition, float targetDistance, Faction enemyTroopSide, IDamagable targetPriorityEnemy, bool isBuildingIncludes) // extended parameter : bool isBuildingIncludes // target priority enemy maybe remove ???
    {
        List<MonoBehaviour> enemyControllersList = new List<MonoBehaviour>();

        enemyControllersList.AddRange(GetTroopControllersList(enemyTroopSide)); //GetEnemyListInRange(troopPosition, targetDistance, enemyTroopSide)

        if (isBuildingIncludes == true)
            enemyControllersList.AddRange(_buildingControllersEnemyList);

        MonoBehaviour targetEnemy = null;

        float closestDistance = Mathf.Infinity;

        foreach (MonoBehaviour enemy in enemyControllersList)
        {
            Vector3 currentEnemyPosition = enemy.transform.position;

            float currentDistanceBetweenEnemy = Vector3.Distance(troopPosition, currentEnemyPosition);

            if (targetPriorityEnemy != null && currentDistanceBetweenEnemy <= targetDistance && enemy.GetComponent<IDamagable>().Equals(targetPriorityEnemy))
                return enemy;

            if (currentDistanceBetweenEnemy <= targetDistance && currentDistanceBetweenEnemy < closestDistance && enemy.GetComponent<IDamagable>() != null) // && isEnemyInAttackRange(troopPosition, currentEnemyPosition, targetDistance))
            {
                targetEnemy = enemy;
                closestDistance = currentDistanceBetweenEnemy;
            }
        }

        return targetEnemy;
    }

    private bool isEnemyInAttackRange(Vector3 startPoint, Vector3 finalPoint, float raycastDistance) // to do
    {
        Vector3 direction = finalPoint - startPoint;

        if (Physics.Raycast(startPoint, direction, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out IDamagable _))
                return true;
        }

        return false;
    }

    public Transform[] GetRandomEnemyPatrollingPoints(int pointsCount)
    {
        if (pointsCount > _enemyPatrollingPoins.Count)
            return null;

        return _enemyPatrollingPoins
            .OrderBy(x => Random.value)
            .Take(pointsCount)
            .ToArray();
    }

    #region Player & Enemy Lists

    private List<TroopController> GetTroopControllersList(Faction troopSide)
            => _troopsBySide[troopSide];

    public List<TroopController> GetPlayerTroopControllersList()
        => _troopsBySide[Faction.Allies];

    public List<TroopController> GetEnemyTroopControllersList()
        => _troopsBySide[Faction.Enemies];

    #endregion

    #region Lists Operations

    private void AddTroopToList(TroopController troopController, Faction troopSide)
    {
        _troopsBySide[troopSide].Add(troopController);
    }

    private void RemoveTroopFromList(TroopController troopController, Faction troopSide)
    {
        _troopsBySide[troopSide].Remove(troopController);
    }

    private void AddBuildingToList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Add(buildingController);
    }

    private void RemoveBuildingFromList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Remove(buildingController);
    }

    #endregion
}
