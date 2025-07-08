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
        GameEvents.instance.OnTroopEnterAnyMode += AssignControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDiedSimple += UpdateTroopStatus;
        GameEvents.instance.OnBuildingDestroyed += UpdateTroopStatus;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopEnterAnyMode -= AssignControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDiedSimple -= UpdateTroopStatus;
        GameEvents.instance.OnBuildingDestroyed -= UpdateTroopStatus;
    }

    #endregion

    public void ChangeTroopControllerAndState()
    {
        if (_selectedOrderMode == OrderMode.None) 
            NoSelectedController();

        else SelectedOrderTroopAction();
    }

    private void NoSelectedController()
    {
        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedMask = (1 << hitLayer);

        CancelEnteringModeAndDisableMenu();

        if (isShiftedMaskOverLayer(shiftedMask, _troopsLayer) && isComponentExists(hit, out TroopController troopController))
        {
            troopController.UIController.OpenTroopGeneralMenu();
            _selectedController = troopController;
        }
        else if (isShiftedMaskOverLayer(shiftedMask, _buildingsLayer) && isComponentExists(hit, out BuildingController buildingController))
        {
            buildingController.UIController.OpenTroopGeneralMenu();
            _selectedController = buildingController;
        }
    }

    private void SelectedOrderTroopAction()
    {
        if (_selectedController is not PlayerTroopController)
            return;

        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        PlayerTroopController playerTroopController = _selectedController as PlayerTroopController;
        TroopStateController playerTroopStateController = playerTroopController.StateController;

        LayerMask hitLayer = hit.collider.gameObject.layer;
        int shiftedLayerMask = (1 << hitLayer);

        Vector3 targetPoint = hit.point;

        switch (_selectedOrderMode)
        {
            case OrderMode.Move:

                if (isShiftedMaskOverLayer(shiftedLayerMask, _terrainLayer))
                    playerTroopStateController.ActivateMoveState(targetPoint, null);

                break;
            case OrderMode.Attack:

                if (isShiftedMaskOverLayer(shiftedLayerMask, _troopsLayer) && isComponentExists(hit, out EnemyTroopController enemy))
                    ActivateAttackState(enemy, playerTroopStateController); // instead - enemy.UIController.OpenAttackMenu();

                else if (isShiftedMaskOverLayer(shiftedLayerMask, _buildingsLayer) && isComponentExists(hit, out BuildingController building))
                    ActivateAttackState(building, playerTroopStateController); // instead - building.UIController.OpenAttackMenu();

                break;
        }

        if (playerTroopController.GetCanvasActivityStateAfterOrder())
            GameEvents.instance.DisableActiveCanvases();

        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #region Helper Methods

    private bool isShiftedMaskOverLayer(int shiftedMask, int layer)
        => (shiftedMask & layer) != 0;

    private bool isComponentExists<T>(RaycastHit hit, out T component) where T : MonoBehaviour
        => hit.collider.TryGetComponent(out component);

    #endregion

    #region Activate Attack

    private void ActivateAttackState<Target>(Target target, TroopStateController troopStateController) where Target : MonoBehaviour, IDamagable 
    {
        if (_selectedController is not TroopController)
            return;

        TroopController troopController = _selectedController as TroopController;

        float troopAttackRange = troopController.TroopScriptable.AttackRangeRadius;

        Transform targetTransform = target.transform;

        Vector3 _selectedTroopPosition = _selectedController.transform.position;
        Vector3 targetPoint = targetTransform.position;

        if (Vector3.Distance(targetTransform.position, _selectedTroopPosition) < troopAttackRange)
        {
            Vector3 targetLookAtPosition = new Vector3(targetTransform.position.x, _selectedController.transform.position.y, targetTransform.position.z);
            //_selectedController.transform.LookAt(targetLookAtPosition);    // to do

            troopStateController.ActivateAttackState(target);
        }
        else
        {
            const float distanceDelta = 0.15f;
            const float distanceModifier = 1 - distanceDelta;

            Vector3 direction = (targetPoint - _selectedTroopPosition).normalized;
            targetPoint -= direction * troopAttackRange * distanceModifier;

            Action action = default;
            action += delegate { troopStateController.ActivateAttackState(target); };

            troopStateController.ActivateMoveState(targetPoint, action);
        }
    }

    #endregion

    #region Raycast

    private RaycastHit GetRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit : default;
    }

    #endregion

    #region Assigning & Updating Order Mode

    private void AssignControllerAndChangeMode(MonoBehaviour troopController, OrderMode orderMode)
    {
        _selectedController = troopController;
        _selectedOrderMode = orderMode;
    }

    private void UpdateTroopStatus(MonoBehaviour controller)
    {
        if (_selectedController == controller)
            AssignControllerAndChangeMode(null, OrderMode.None);
    }

    private void CancelEnteringModeAndDisableMenu()
    {
        GameEvents.instance.DisableActiveCanvases();

        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #endregion
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