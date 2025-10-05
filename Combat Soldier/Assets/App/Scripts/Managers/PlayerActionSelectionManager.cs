using UnityEngine;

public class PlayerActionSelectionManager : MonoBehaviour, IInitializeManager
{
    [Header("Raycast Layers")]

    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _attackTargetLayers = default;

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


        GameEvents.instance.OnTroopDiedUI += UpdateTroopStatus;
        GameEvents.instance.OnTroopDisableUI += UpdateTroopStatus;
        GameEvents.instance.OnBuildingDestroyed += UpdateTroopStatus;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopEnterAnyMode -= AssignControllerAndChangeMode;
        GameEvents.instance.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        GameEvents.instance.OnTroopDiedUI -= UpdateTroopStatus;
        GameEvents.instance.OnTroopDisableUI -= UpdateTroopStatus;
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

        CancelEnteringModeAndDisableMenu();

        if (isLayerInMask(hitLayer, _attackTargetLayers) && isComponentExists(hit, out IDamagable enemyDamagable))
        {
            MonoBehaviour currentController = enemyDamagable as MonoBehaviour;
            GameEvents.instance.OpenTroopMenu(currentController);

            _selectedController = currentController;
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

        Vector3 targetPoint = hit.point;

        switch (_selectedOrderMode)
        {
            case OrderMode.Move:

                if (isLayerInMask(hitLayer, _terrainLayer))
                    playerTroopStateController.ActivateMoveState(targetPoint);

                break;
            case OrderMode.Attack:

                if (isLayerInMask(hitLayer, _attackTargetLayers) && isComponentExists(hit, out IDamagable enemyDamagable))
                    ActivateAttackState(enemyDamagable, playerTroopStateController);

                break;
        }

        if (playerTroopController.GetCanvasActivityStateAfterOrder())
            GameEvents.instance.DisableActiveCanvases();

        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #region Helper Methods

    private bool isLayerInMask(int layer, int mask)
        => ((1 << layer) & mask) != 0;

    private bool isComponentExists<T>(RaycastHit hit, out T component)
        => hit.collider.TryGetComponent(out component);

    #endregion

    #region Activate Attack

    private void ActivateAttackState(IDamagable enemyDamagable, TroopStateController troopStateController)
    {
        if (_selectedController is not PlayerTroopController)
            return;

        PlayerTroopController troopController = _selectedController as PlayerTroopController;
        MonoBehaviour enemyMonoBehaviour = enemyDamagable as MonoBehaviour;

        Vector3 targetPosition = enemyMonoBehaviour.transform.position;
        Vector3 troopPosition = _selectedController.transform.position;

        Vector3 targetPoint = targetPosition;

        float troopAttackRange = troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(targetPosition, troopPosition) <= troopAttackRange)
        {
            Vector3 targetLookAtPosition = new Vector3(targetPosition.x, troopPosition.y, targetPosition.z);
            _selectedController.transform.LookAt(targetLookAtPosition);

            troopStateController.ActivateAttackState(enemyDamagable);
        }
        else
        {
            const float distanceDelta = 0.15f;
            const float distanceModifier = 1 - distanceDelta;

            Vector3 direction = (targetPoint - troopPosition).normalized;
            targetPoint -= direction * troopAttackRange * distanceModifier;

            troopStateController.ActivateMoveState(targetPoint);
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
        if (_selectedController != controller)
            return;

        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    private void CancelEnteringModeAndDisableMenu()
    {
        GameEvents.instance.DisableActiveCanvases();
        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #endregion
}

public enum OrderMode
{
    None,
    Move,
    Attack,
    etc
}