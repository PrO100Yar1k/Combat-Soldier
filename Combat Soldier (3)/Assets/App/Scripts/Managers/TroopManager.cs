using System;
using UnityEngine;

public class TroopManager : MonoBehaviour
{
    [Header("Raycast Layers")]

    [SerializeField] private LayerMask _terrainLayer = default;

    [SerializeField] private LayerMask _troopsLayer = default;

    private TroopController _selectedTroopController = default;

    private OrderMode _selectedOrderMode = default;

    #region Events & Initialization

    public void InitializeManager()
        => SubscribeToEvents();

    private void OnDisable()
        => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopEnterAnyMode += AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDied += UpdateTroopStatus;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopEnterAnyMode -= AssignTroopControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDied -= UpdateTroopStatus;
    }

    #endregion

    public void ChangeTroopControllerAndState()
    {
        if (_selectedOrderMode == OrderMode.None)
            NoSelectedOrderTroopAction();
        else 
            SelectedOrderTroopAction();
    }


    private void NoSelectedOrderTroopAction()
    {
        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedMask = (1 << hitLayer);

        if (_selectedTroopController != null)
            CancelEnteringModeAndDisableMenu();

        if ((shiftedMask & _troopsLayer.value) != 0 && hit.collider.TryGetComponent(out TroopController troopController))
        {
            _selectedTroopController = troopController;
            _selectedTroopController.UIController.OpenTroopGeneralMenu();
        }
    }

    private void SelectedOrderTroopAction()
    {
        if (_selectedTroopController == null)
            return;

        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        TroopStateController troopStateController = _selectedTroopController.StateController;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedMask = (1 << hitLayer);

        Vector3 targetPoint = hit.point;

        if ((shiftedMask & _terrainLayer.value) != 0 && _selectedOrderMode == OrderMode.Move)
        {
            troopStateController.ActivateMoveState(_selectedTroopController, targetPoint, null);
        }
        else if ((shiftedMask & _troopsLayer.value) != 0 && _selectedOrderMode == OrderMode.Attack && hit.collider.TryGetComponent(out EnemyTroopController enemy))
        {
            ActivateAttackState(enemy, enemy.transform.position, troopStateController);
            //_selectedTroopController.UIController.OpenAttackMenu();
        }

        CancelEnteringModeAndDisableMenu();
    }

    private RaycastHit GetRaycastHit()
    {
        RaycastHit hit;

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Physics.Raycast(ray, out hit);

        return hit;
    }

    private void ActivateAttackState(EnemyTroopController enemy, Vector3 targetPoint, TroopStateController troopStateController)
    {
        Vector3 direction = (targetPoint - _selectedTroopController.transform.position).normalized;
        float troopAttackRange = _selectedTroopController.TroopScriptable.AttackRangeRadius;

        const float distanceModifier = 0.8f; // could be changed a little bit

        targetPoint -= direction * troopAttackRange * distanceModifier;

        Action action = default;
        action += delegate { troopStateController.ActivateAttackState(enemy); } ;

        troopStateController.ActivateMoveState(_selectedTroopController, targetPoint, action);
    }

    private void AssignTroopControllerAndChangeMode(TroopController troopController, OrderMode orderMode)
    {
        _selectedTroopController = troopController;
        _selectedOrderMode = orderMode;
    }

    private void CancelEnteringModeAndDisableMenu()
    {
        if (_selectedTroopController != null)
            GameEvents.instance.TroopDisableCanvases();

        AssignTroopControllerAndChangeMode(null, OrderMode.None);
    }

    private void UpdateTroopStatus(TroopController troopController, TroopSide troopSide)
    {
        if (_selectedTroopController == troopController)
            AssignTroopControllerAndChangeMode(null, OrderMode.None);
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