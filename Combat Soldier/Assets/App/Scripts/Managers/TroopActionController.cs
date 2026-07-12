using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class TroopActionController : MonoBehaviour, ITroopSelection
{
    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _attackTargetLayers = default;

    private MonoBehaviour _selectedController = default;
    private OrderMode _selectedOrderMode = default;

    private GameEventBus _gameEventBus = default;

    #region Events & Initialization

    public void OnEnable()
        => SubscribeToEvents();

    private void OnDisable()
        => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        _gameEventBus.OnTroopEnterAnyMode += AssignControllerAndChangeMode;
        _gameEventBus.OnTroopCancelEnteringMode += CancelEnteringModeAndDisableMenu;

        _gameEventBus.OnTroopDiedUI += UpdateTroopStatus;
        _gameEventBus.OnTroopDisableUI += UpdateTroopStatus;
        _gameEventBus.OnBuildingDestroyed += UpdateTroopStatus;
    }

    private void UnSubscribeFromEvents()
    {
        _gameEventBus.OnTroopEnterAnyMode -= AssignControllerAndChangeMode;
        _gameEventBus.OnTroopCancelEnteringMode -= CancelEnteringModeAndDisableMenu;

        _gameEventBus.OnTroopDiedUI -= UpdateTroopStatus;
        _gameEventBus.OnTroopDisableUI -= UpdateTroopStatus;
        _gameEventBus.OnBuildingDestroyed -= UpdateTroopStatus;
    }

    #endregion

    [Inject]
    public void Construct(GameEventBus gameEvents)
    {
        _gameEventBus = gameEvents;
    }

    public void SelectTroopOrderState()
    {
        if (_selectedOrderMode == OrderMode.None) 
            NoSelectedController();

        else SelectedOrderTroopAction();
    }

    #region Troop Controller Selection 

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
            _gameEventBus.OpenTroopMenu(currentController);

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
            _gameEventBus.DisableActiveCanvases();

        AssignControllerAndChangeMode(null, OrderMode.None);
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
        _gameEventBus.DisableActiveCanvases();
        AssignControllerAndChangeMode(null, OrderMode.None);
    }

    #endregion

    #region Activate Attack

    private void ActivateAttackState(IDamagable enemyDamagable, TroopStateController troopStateController)
    {
        if (_selectedController is not PlayerTroopController)
            return;

        if (enemyDamagable.GetFaction() == Faction.Allies)
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

    #region Helper Methods

    private bool isLayerInMask(int layer, int mask)
        => ((1 << layer) & mask) != 0;

    private bool isComponentExists<T>(RaycastHit hit, out T component)
        => hit.collider.TryGetComponent(out component);

    private RaycastHit GetRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit : default;
    }

    #endregion
}