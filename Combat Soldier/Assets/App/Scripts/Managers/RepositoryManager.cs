using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class RepositoryManager : System.IDisposable
{
    private List<TroopController> _troopControllersPlayerList = new List<TroopController>();
    private List<TroopController> _troopControllersEnemyList = new List<TroopController>();

    private List<BuildingController> _buildingControllersEnemyList = new List<BuildingController>();

    private readonly GameEvents _gameEventBus;
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

    public RepositoryManager(GameEvents gameEvents, [Inject(Id = "Enemy Points")] List<Transform> enemyPatrollingPoints)
    {
        _gameEventBus = gameEvents;
        _enemyPatrollingPoins = new List<Transform>(enemyPatrollingPoints);

        SubscribeToEvents();
    }

    public void InitializeAllTroops()
    {
        foreach (TroopController controller in _troopControllersPlayerList)
            controller.InitializeTroop();

        foreach (TroopController controller in _troopControllersEnemyList)
            controller.InitializeTroop();
    }

    public void InitializeAllBuildings()
    {
        foreach (BuildingController controller in _buildingControllersEnemyList)
            controller.InitializeBuilding();
    }

    // make extension methods
    public TroopController[] GetEnemyListInRange(Vector3 troopPosition, float troopRange, TroopSide enemyTroopSide)
    {
        return GetTroopControllersList(enemyTroopSide)
            .Where(troop => Vector3.Distance(troopPosition, troop.transform.position) <= troopRange)
            .ToArray();
    }

    public MonoBehaviour GetClosestEnemyInRange(Vector3 troopPosition, float targetDistance, TroopSide enemyTroopSide, IDamagable targetPriorityEnemy, bool isBuildingIncludes) // extended parameter : bool isBuildingIncludes // target priority enemy maybe remove ???
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

    public List<TroopController> GetTroopControllersList(TroopSide troopSide)
        => troopSide == TroopSide.Player ? _troopControllersPlayerList : _troopControllersEnemyList;

    public List<TroopController> GetPlayerTroopControllersList()
        => _troopControllersPlayerList;

    public List<TroopController> GetEnemyTroopControllersList()
        => _troopControllersEnemyList;

    #endregion

    #region Lists Operations

    private void AddTroopToList(TroopController troopController, TroopSide troopSide)
    {
        GetTroopControllersList(troopSide).Add(troopController);
    }

    private void RemoveTroopFromList(TroopController troopController, TroopSide troopSide)
    {
        GetTroopControllersList(troopSide).Remove(troopController);
    }

    private void AddBuildingToList(BuildingController buildingController)
    {
        Debug.Log("pupuou");

        _buildingControllersEnemyList.Add(buildingController);
    }

    private void RemoveBuildingFromList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Remove(buildingController);
    }

    #endregion
}
