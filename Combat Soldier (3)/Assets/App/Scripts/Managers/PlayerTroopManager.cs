using System;
using UnityEngine;

public class PlayerTroopManager : MonoBehaviour, IInitializeManager
{
    [Header("Raycast Layers")] [Space(3)]

    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _troopsLayer = default;
    [SerializeField] private LayerMask _buildingsLayer = default;

    private MonoBehaviour _selectedController = default;
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

    // to do add region to this script

    public void ChangeTroopControllerAndState()
    {
        if (_selectedOrderMode == OrderMode.None) 
            NoSelectedOrderAction();

        else SelectedOrderTroopAction(); // could be used only for troops
    }


    private void NoSelectedOrderAction()
    {
        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedMask = (1 << hitLayer);

        if (_selectedController != null)
            CancelEnteringModeAndDisableMenu();

        if ((shiftedMask & _troopsLayer.value) != 0 && hit.collider.TryGetComponent(out TroopController troopController))
        {
            troopController.UIController.OpenTroopGeneralMenu();
            _selectedController = troopController;
        }
        else if ((shiftedMask & _buildingsLayer.value) != 0 && hit.collider.TryGetComponent(out BuildingController buildingController))
        {
            buildingController.UIController.OpenTroopGeneralMenu();
            _selectedController = buildingController;
        }
    }

    private void SelectedOrderTroopAction()
    {
        if (_selectedController is not TroopController)
            return;

        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        TroopController troopController = _selectedController as TroopController;
        TroopStateController troopStateController = troopController.StateController;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedMask = (1 << hitLayer);

        Vector3 targetPoint = hit.point;

        if ((shiftedMask & _terrainLayer.value) != 0 && _selectedOrderMode == OrderMode.Move)
        {
            troopStateController.ActivateMoveState(targetPoint, null);
        }
        else if (_selectedOrderMode == OrderMode.Attack)
        {
            if ((shiftedMask & _troopsLayer.value) != 0 && hit.collider.TryGetComponent(out EnemyTroopController enemy))
            {
                ActivateAttackState(enemy, troopStateController);   //_selectedTroopController.UIController.OpenAttackMenu();
            }
            else if ((shiftedMask & _buildingsLayer.value) != 0 && hit.collider.TryGetComponent(out BuildingController building))
            {
                ActivateAttackState(building, troopStateController);
            }
        }

        CancelEnteringModeAndDisableMenu();
    }

    private void ActivateAttackState<Target>(Target target, TroopStateController troopStateController) where Target : MonoBehaviour, IDamagable 
    {
        if (_selectedController is not TroopController)
            return;

        TroopController troopController = _selectedController as TroopController;

        Vector3 _selectedTroopPosition = _selectedController.transform.position;
        float troopAttackRange = troopController.TroopScriptable.AttackRangeRadius;

        Transform targetTransform = target.transform;
        Vector3 targetPoint = targetTransform.position;

        if (Vector3.Distance(targetTransform.position, _selectedTroopPosition) < troopAttackRange)
        {
            _selectedController.transform.LookAt(targetTransform); // ?
            troopStateController.ActivateAttackState(target);
        }
        else
        {
            const float distanceModifier = 0.85f; // could be changed a little bit
            Vector3 direction = (targetPoint - _selectedTroopPosition).normalized;

            targetPoint -= direction * troopAttackRange * distanceModifier;

            Action action = default;
            action += delegate { troopStateController.ActivateAttackState(target); };

            troopStateController.ActivateMoveState(targetPoint, action);
        }
    }

    private RaycastHit GetRaycastHit()
    {
        RaycastHit hit;

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Physics.Raycast(ray, out hit);

        return hit;
    }


    private void AssignTroopControllerAndChangeMode(TroopController troopController, OrderMode orderMode)
    {
        _selectedController = troopController;
        _selectedOrderMode = orderMode;
    }

    private void UpdateTroopStatus(TroopController troopController, TroopSide troopSide)
    {
        if (_selectedController == troopController)
            AssignTroopControllerAndChangeMode(null, OrderMode.None);
    }

    private void CancelEnteringModeAndDisableMenu()
    {
        if (_selectedController != null)
            GameEvents.instance.TroopDisableCanvases();

        AssignTroopControllerAndChangeMode(null, OrderMode.None);
    }
}

public enum TroopType
{
    Soldier_Type_1,
    Soldier_Type_2,
    AntiTank_Soldier,
    etc_1,
    etc_2
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
    Attack,
    etc
}