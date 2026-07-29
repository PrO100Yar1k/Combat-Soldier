using Assets.App.Scripts;
using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Infrastructure.Others;
using UnityEngine;
using Zenject;

public class PlayerCommandController : MonoBehaviour
{
    [SerializeField] private LayerMask _terrainLayer = default;
    [SerializeField] private LayerMask _attackTargetLayers = default; //

    private PlayerTroopController _controlledTroop;
    private GameEventBus _gameEventBus;

    [Inject]
    public void Construct(GameEventBus gameEvents)
    {
        _gameEventBus = gameEvents;
    }

    private void OnEnable()
    {
        _gameEventBus.OnDeselectController += ClearControlledTroop;
        _gameEventBus.OnOpenTroopMenu += SetControlledTroop;

        _gameEventBus.OnTroopDiedUI += HandleTroopRemoval;
        _gameEventBus.OnTroopDisableUI += HandleTroopRemoval;
        _gameEventBus.OnBuildingDestroyed += HandleTroopRemoval;
    }

    private void OnDisable()
    {
        _gameEventBus.OnDeselectController -= ClearControlledTroop;
        _gameEventBus.OnOpenTroopMenu -= SetControlledTroop;

        _gameEventBus.OnTroopDiedUI -= HandleTroopRemoval;
        _gameEventBus.OnTroopDisableUI -= HandleTroopRemoval;
        _gameEventBus.OnBuildingDestroyed -= HandleTroopRemoval;
    }

    public void SetControlledTroop(MonoBehaviour controller)
    {
        if (controller is PlayerTroopController playerTroop)
            _controlledTroop = playerTroop;
    }

    public void ClearControlledTroop()
    {
        _controlledTroop = null;
    }

    public void ExecuteCommand()
    {
        if (_controlledTroop == null)
            return;

        RaycastHit hit = GetRaycastHit();

        if (hit.collider == null)
            return;

        TroopStateController stateController = _controlledTroop.StateController;

        int hitLayer = hit.collider.gameObject.layer;
        Vector3 targetPoint = hit.point;

        if (IsLayerInMask(hitLayer, _attackTargetLayers) && hit.collider.TryGetComponent(out IDamagable enemy))
        {
            if (enemy.GetFaction() != Faction.Allies)
            {
                ActivateAttackState(enemy, stateController);
                FinishCommandExecution();
                return;
            }
        }

        if (IsLayerInMask(hitLayer, _terrainLayer))
        {
            stateController.ActivateMoveState(targetPoint);
            FinishCommandExecution();
        }
    }

    private void ActivateAttackState(IDamagable enemy, TroopStateController stateController)
    {
        if (enemy is not MonoBehaviour enemyMono)
            return;

        Vector3 targetPos = enemyMono.transform.position;
        Vector3 troopPos = _controlledTroop.transform.position;

        float attackRange = _controlledTroop.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(targetPos, troopPos) <= attackRange)
        {
            _controlledTroop.transform.LookAt(new Vector3(targetPos.x, troopPos.y, targetPos.z));
            stateController.ActivateAttackState(enemy);
        }
        else
        {
            Vector3 targetPoint = CombatMath.GetAttackDestination(troopPos, targetPos, attackRange);
            stateController.ActivateMoveState(targetPoint);
        }
    }

    private void FinishCommandExecution()
    {
        if (_controlledTroop.GetCanvasActivityState())
            _gameEventBus.DisableActiveCanvas();

        // make unit circle active (check out canvas ui layouts)
    }

    private void HandleTroopRemoval(MonoBehaviour controller)
    {
        if (_controlledTroop == controller)
        {
            ClearControlledTroop();
        }
    }

    private bool IsLayerInMask(int layer, int mask)
    {
        return ((1 << layer) & mask) != 0;
    }

    private RaycastHit GetRaycastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit : default;
    }
}