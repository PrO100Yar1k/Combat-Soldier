using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TroopManager : MonoBehaviour // code refactoring
{
    [Header("Layers")] [Space(3)]

    [SerializeField] private LayerMask _terrainLayer = default;

    [SerializeField] private LayerMask _troopsLayer = default;

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
        GameEvents.instance.OnTroopEnterAnyMode += AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopEnterAnyMode -= AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;
    }

    #endregion

    private void Update()
    {
        // check with new input system

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
                GameObject hitObject = hit.collider.gameObject;
                int shiftedMask = (1 << hitObject.layer);

                var troopStateController = _selectedTroopController.StateController;

                Vector3 targetPoint = hit.point;

                if ((shiftedMask & _terrainLayer.value) != 0 && _selectedOrderMode == OrderMode.Move) {
                    troopStateController.ActivateMoveState(targetPoint, null);
                }
                else if ((shiftedMask & _troopsLayer.value) != 0 && _selectedOrderMode == OrderMode.Attack && hitObject.TryGetComponent(out EnemyTroopController enemy)) {
                    //_selectedTroopController.UIController.OpenAttackMenu();
                    ActivateAttackState(enemy, enemy.transform.position, troopStateController);
                }
            }
        }
        _selectedOrderMode = OrderMode.None;
    }

    private void ActivateAttackState(EnemyTroopController enemy, Vector3 targetPoint, TroopStateController _troopStateController) // to do
    {
        Vector3 direction = (targetPoint - _selectedTroopController.transform.position).normalized;
        float troopAttackRange = _selectedTroopController.TroopScriptable.AttackRangeRadius;

        const float distanceModifier = 0.8f; // could be changed a little bit

        targetPoint -= direction * troopAttackRange * distanceModifier;

        Action action = default;
        action += delegate { _troopStateController.ActivateAttackState(enemy); } ;

        _troopStateController.ActivateMoveState(targetPoint, action);
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