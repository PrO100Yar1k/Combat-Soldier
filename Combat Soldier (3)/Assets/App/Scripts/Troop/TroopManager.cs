using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TroopManager : MonoBehaviour
{
    [Header("Layers")] [Space(3)]

    [SerializeField] private LayerMask _terrainLayer = default;

    [SerializeField] private LayerMask _troopsLayer = default;

    private List<TroopController> _troopControllersPlayerList = new List<TroopController>();
    private List<TroopController> _troopControllersEnemyList = new List<TroopController>();

    private TroopController _selectedTroopController = default;

    private OrderMode _selectedOrderMode = default;

    private RaycastHit hit;

    #region Events & Initialization

    public void InitializeManager()
        => SubscribeToEvents();

    private void OnDisable()
        => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopSpawned += AddTroopToList;

        GameEvents.instance.OnTroopEnterAnyMode += AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDied += RemoveTroopFromList;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopSpawned -= AddTroopToList;

        GameEvents.instance.OnTroopEnterAnyMode -= AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDied -= RemoveTroopFromList;
    }

    #endregion

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !IsPointerOverUI())  //IsPointerOverUI() MUST BE ALWAYS ON FALSE
        {  
            if (_selectedOrderMode == OrderMode.None) {
                NoSelectedTroopRaycast();
            } 
            else SelectedTroopRaycastAction();
        }
    }

    private void NoSelectedTroopRaycast() // 
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                LayerMask hitObjectLayer = hit.collider.gameObject.layer;
                int shiftedMask = (1 << hitObjectLayer);

                if ((shiftedMask & _troopsLayer.value) != 0 && hit.collider.TryGetComponent(out TroopController troopController))
                {
                    _selectedTroopController = troopController;
                    _selectedTroopController.UIController.OpenTroopActionMenu();
                }
                else if (_selectedTroopController != null)
                    CancelEnteringModeAndDisableMenu();
            }
        }
    }

    private void SelectedTroopRaycastAction() // to do
    {
        if (_selectedTroopController == null)
            return;

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                LayerMask hitObjectLayer = hit.collider.gameObject.layer;
                int shiftedMask = (1 << hitObjectLayer);

                var _troopStateController = _selectedTroopController.StateController;

                if ((shiftedMask & _terrainLayer.value) != 0 && _selectedOrderMode == OrderMode.Move) {
                    _troopStateController.ActivateMoveState();
                    GameEvents.instance.TroopMovement(hit.point);
                }
                else if ((shiftedMask & _troopsLayer.value) != 0 && _selectedOrderMode == OrderMode.Attack && _selectedTroopController != hit.collider) {
                    _selectedTroopController.UIController.OpenAttackMenu();
                }
            }
        }
        _selectedOrderMode = OrderMode.None;
    }

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

    private void AssignTroopControllerAndChangeMode(TroopController troopController, OrderMode orderMode) // think about namespacing
    {
        _selectedTroopController = troopController;
        _selectedOrderMode = orderMode;
    }

    private void CancelEnteringModeAndDisableMenu()
    {
        if (_selectedTroopController != null)
            GameEvents.instance.DisableCanvases();

        //_selectedTroopController.UIController.ChangeCanvasActivationState(false);

        AssignTroopControllerAndChangeMode(null, OrderMode.None);
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
            if (results[i].gameObject.layer == 5)
                return true;

        return false;
    }


    public List<TroopController> GetTroopControllersList(TroopSide troopSide)
        => troopSide == TroopSide.Player ? _troopControllersPlayerList : _troopControllersEnemyList;


    public TroopController[] GetEnemyInCertainRange(Vector3 troopPosition, float troopRange, TroopSide enemyTroopSide)
    {
        List<TroopController> enemyControllersList = new List<TroopController>();
        List<TroopController> troopControllersList = new List<TroopController>(GetTroopControllersList(enemyTroopSide));

        foreach (TroopController troopController in troopControllersList)
        {
            Vector3 currentEnemyPosition = troopController.transform.position;

            if (Vector3.Distance(troopPosition, currentEnemyPosition) <= troopRange) {
                enemyControllersList.Add(troopController);
            }
        }

        return enemyControllersList.ToArray();
    }

    public TroopController GetEnemyInCertainRange(Vector3 troopPosition, TroopController[] enemyControllersList, TroopController targetPriorityEnemy)
    {
        float closestDistance = Mathf.Infinity;
        TroopController targetEnemy = default;

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
}

public enum TroopType
{
    Soldier_Type_1,
    Soldier_Type_2,
    AntiTank_Soldier
}

public enum AttackType
{
    Land,
    Air,
    Both
}

public enum OrderMode
{
    None,
    Move,
    Attack
}