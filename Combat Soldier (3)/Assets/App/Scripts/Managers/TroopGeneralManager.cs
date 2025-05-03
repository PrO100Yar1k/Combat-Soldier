using System.Collections.Generic;
using UnityEngine;

public class TroopGeneralManager : MonoBehaviour
{
    private List<TroopController> _troopControllersPlayerList = new List<TroopController>();
    private List<TroopController> _troopControllersEnemyList = new List<TroopController>();

    #region Singleton Activation & Initialization

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

        GameEvents.instance.OnTroopDied += RemoveTroopFromList;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopSpawned -= AddTroopToList;

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

    public TroopController GetClosestEnemyInRange(Vector3 troopPosition, TroopSide enemyTroopSide, float troopRange, TroopController targetPriorityEnemy)
    {
        TroopController[] enemyControllersList = GetEnemyListInRange(troopPosition, troopRange, enemyTroopSide);

        TroopController targetEnemy = default;
        float closestDistance = Mathf.Infinity;

        foreach (TroopController enemyController in enemyControllersList)
        {
            Vector3 currentEnemyPosition = enemyController.transform.position;

            float currentDistanceBetweenEnemy = Vector3.Distance(troopPosition, currentEnemyPosition);

            if (enemyController == targetPriorityEnemy)
                return targetPriorityEnemy;

            if (currentDistanceBetweenEnemy < closestDistance)
            {
                targetEnemy = enemyController;
                closestDistance = currentDistanceBetweenEnemy;
            }
        }

        return targetEnemy;
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

    #endregion
}
