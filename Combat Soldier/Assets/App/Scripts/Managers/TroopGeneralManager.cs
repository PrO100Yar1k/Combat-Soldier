using System.Collections.Generic;
using UnityEngine;

public class TroopGeneralManager : MonoBehaviour, IInitializeManager
{
    private List<TroopController> _troopControllersPlayerList = new List<TroopController>();
    private List<TroopController> _troopControllersEnemyList = new List<TroopController>();

    private List<BuildingController> _buildingControllersEnemyList = new List<BuildingController>();

    #region Initialization & Singleton

    [HideInInspector] public static TroopGeneralManager instance;

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
        GameEvents.instance.OnBuildingSpawned += AddBuildingToList;

        GameEvents.instance.OnBuildingDestroyed += RemoveBuildingFromList;
        GameEvents.instance.OnTroopDied += RemoveTroopFromList;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopSpawned -= AddTroopToList;
        GameEvents.instance.OnBuildingSpawned -= AddBuildingToList;

        GameEvents.instance.OnBuildingDestroyed += RemoveBuildingFromList;
        GameEvents.instance.OnTroopDied -= RemoveTroopFromList;
    }

    #endregion

    public TroopController[] GetEnemyListInRange(Vector3 troopPosition, float troopRange, TroopSide enemyTroopSide)
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

    public MonoBehaviour GetClosestEnemyInRange(Vector3 troopPosition, TroopSide enemyTroopSide, float troopAttackRange, IDamagable targetPriorityEnemy)
    {
        List<MonoBehaviour> enemyControllersList = new List<MonoBehaviour>();

        enemyControllersList.AddRange(GetEnemyListInRange(troopPosition, troopAttackRange, enemyTroopSide));
        enemyControllersList.AddRange(_buildingControllersEnemyList);

        MonoBehaviour targetEnemy = default;

        float closestDistance = Mathf.Infinity;

        foreach (MonoBehaviour enemy in enemyControllersList)
        {
            Vector3 currentEnemyPosition = enemy.transform.position;

            float currentDistanceBetweenEnemy = Vector3.Distance(troopPosition, currentEnemyPosition);

            if (currentDistanceBetweenEnemy <= troopAttackRange && enemy.GetComponent<IDamagable>().Equals(targetPriorityEnemy))
                return enemy;

            if (currentDistanceBetweenEnemy < closestDistance && isEnemyInAttackRange(troopPosition, currentEnemyPosition, troopAttackRange))
            {
                targetEnemy = enemy;
                closestDistance = currentDistanceBetweenEnemy;
            }
        }

        return targetEnemy;
    }

    private bool isEnemyInAttackRange(Vector3 startPoint, Vector3 finalPoint, float raycastDistance)
    {
        Vector3 direction = finalPoint - startPoint;

        if (Physics.Raycast(startPoint, direction, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out IDamagable _))
                return true;
        }

        return false;
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

        Debug.Log("Troop successfully added!");
    }

    private void RemoveTroopFromList(TroopController troopController, TroopSide troopSide)
    {
        GetTroopControllersList(troopSide).Remove(troopController);

        Debug.Log("Troop successfully removed!");
    }

    private void AddBuildingToList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Add(buildingController);

        Debug.Log("Building successfully added!");
    }

    private void RemoveBuildingFromList(BuildingController buildingController)
    {
        _buildingControllersEnemyList.Remove(buildingController);

        Debug.Log("Building successfully removed!");
    }
    #endregion
}
