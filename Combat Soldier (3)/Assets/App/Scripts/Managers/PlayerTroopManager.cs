using System;
using UnityEngine;

public class PlayerTroopManager : MonoBehaviour, IInitializeManager
{
    [Header("Raycast Layers")] [Space(3)]

    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _troopsLayer = default;
    [SerializeField] private LayerMask _buildingsLayer = default;

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

    // to do add region to this script

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
            troopStateController.ActivateMoveState(targetPoint, null);
        }
        else if (_selectedOrderMode == OrderMode.Attack)
        {
            if ((shiftedMask & _troopsLayer.value) != 0 && hit.collider.TryGetComponent(out EnemyTroopController enemy))
            {
                ActivateAttackState(enemy, troopStateController);
                //_selectedTroopController.UIController.OpenAttackMenu();
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
        Vector3 _selectedTroopPosition = _selectedTroopController.transform.position;
        float troopAttackRange = _selectedTroopController.TroopScriptable.AttackRangeRadius;

        Transform targetTransform = target.transform;
        Vector3 targetPoint = targetTransform.position;

        IDamagable targetDamagable = (IDamagable) target;

        if (Vector3.Distance(targetTransform.position, _selectedTroopPosition) < troopAttackRange)
        {
            _selectedTroopController.transform.LookAt(targetTransform); // ?
            troopStateController.ActivateAttackState(targetDamagable);
        }
        else
        {
            const float distanceModifier = 0.85f; // could be changed a little bit
            Vector3 direction = (targetPoint - _selectedTroopPosition).normalized;

            targetPoint -= direction * troopAttackRange * distanceModifier;

            Action action = default;
            action += delegate { troopStateController.ActivateAttackState(targetDamagable); };

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