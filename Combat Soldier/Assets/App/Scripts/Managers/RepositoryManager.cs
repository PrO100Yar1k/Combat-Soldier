using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RepositoryManager : MonoBehaviour, IInitializeManager
{
    [SerializeField] private List<Transform> _enemyPatrollingPointList = new List<Transform>();

    private List<TroopController> _troopControllersPlayerList = new List<TroopController>();
    private List<TroopController> _troopControllersEnemyList = new List<TroopController>();

    private List<BuildingController> _buildingControllersEnemyList = new List<BuildingController>();

    #region Initialization & Singleton

    [HideInInspector] public static RepositoryManager instance;

    public void InitializeManager()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Initialize();

        instance = this;
    }

    #endregion

    #region Events

    private void Initialize()
        => SubscribeToEvents();

    private void OnDisable()
        => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopSpawned += AddTroopToList;
        GameEvents.instance.OnTroopDied += RemoveTroopFromList;

        GameEvents.instance.OnBuildingSpawned += AddBuildingToList;
        GameEvents.instance.OnBuildingDestroyed += RemoveBuildingFromList;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopSpawned -= AddTroopToList;
        GameEvents.instance.OnTroopDied -= RemoveTroopFromList;

        GameEvents.instance.OnBuildingSpawned -= AddBuildingToList;
        GameEvents.instance.OnBuildingDestroyed += RemoveBuildingFromList;
    }

    #endregion

    // make extension methods

    public TroopController[] GetEnemyListInRange(Vector3 troopPosition, float troopRange, TroopSide enemyTroopSide) // linq remake
    {
        List<TroopController> enemyControllersList = new List<TroopController>();
        List<TroopController> troopControllersList = new List<TroopController>(GetTroopControllersList(enemyTroopSide));

        foreach (TroopController troopController in troopControllersList)
        {
            Vector3 currentEnemyPosition = troopController.transform.position;

            if (Vector3.Distance(troopPosition, currentEnemyPosition) <= troopRange)
            {
                enemyControllersList.Add(troopController);
            }
        }

        return enemyControllersList.ToArray();
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
        if (pointsCount > _enemyPatrollingPointList.Count)
            return null;

        return _enemyPatrollingPointList
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

        //Debug.Log("Troop successfully added!");
    }

    private void RemoveTroopFromList(TroopController troopController, TroopSide troopSide)
    {
        GetTroopControllersList(troopSide).Remove(troopController);

        //Debug.Log("Troop successfully removed!");
    }

    private void AddBuildingToList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Add(buildingController);

        //Debug.Log("Building successfully added!");
    }

    private void RemoveBuildingFromList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Remove(buildingController);

        //Debug.Log("Building successfully removed!");
    }

    #endregion
}
