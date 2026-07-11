using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class TroopActionController : MonoBehaviour
{
    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _attackTargetLayers = default;

    private MonoBehaviour _selectedController = default;
    private OrderMode _selectedOrderMode = default;

    private GameEvents _gameEvents = default;

    #region Events & Initialization

    public void OnEnable()
        => SubscribeToEvents();

    private void OnDisable()
        => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        _gameEvents.OnTroopEnterAnyMode += AssignControllerAndChangeMode;
        _gameEvents.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;

        _gameEvents.OnTroopDiedUI += UpdateTroopStatus;
        _gameEvents.OnTroopDisableUI += UpdateTroopStatus;
        _gameEvents.OnBuildingDestroyed += UpdateTroopStatus;
    }

    private void UnSubscribeFromEvents()
    {
        _gameEvents.OnTroopEnterAnyMode -= AssignControllerAndChangeMode;
        _gameEvents.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        _gameEvents.OnTroopDiedUI -= UpdateTroopStatus;
        _gameEvents.OnTroopDisableUI -= UpdateTroopStatus;
        _gameEvents.OnBuildingDestroyed -= UpdateTroopStatus;
    }

    #endregion

    [Inject]
    public void Construct(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

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
            _gameEvents.OpenTroopMenu(currentController);

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

        if (playerTroopController.GetCanvasActivityState())
            _gameEvents.DisableActiveCanvases();

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
        _gameEvents.DisableActiveCanvases();
        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #endregion
}