using System.Collections;
using UnityEngine;
using System;

public class EnemyTroopController : TroopController, IReactableForDamage
{
    [SerializeField] protected TroopModelController _troopModelController = default;
    public TroopModelController TroopModelController => _troopModelController;

    protected override void InitializeTroop()
    {
        StateController = new EnemyTroopStateController(this, _screenCanvasController);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);

        _troopModelController.InitializeModelController(this, gameObject);

        //StartCoroutine(FindPlayerUnits());
    }

    private IEnumerator FindPlayerUnits()
    {
        IDamagable targetPriorityEnemy = null;
        TroopSide targetTroopSide = TroopSide.Player;

        float visibleRange = _troopScriptable.ViewRangeRadius;

        while (true)
        {
            const float delay = 1.0f;

            Vector3 currentPosition = transform.position;

            MonoBehaviour closestEnemyInViewRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, visibleRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInViewRange != null)
            {
                IDamagable enemyDamagable = closestEnemyInViewRange as IDamagable;

                Vector3 targetPosition = closestEnemyInViewRange.transform.position;

                //
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        if (target == null)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPos = target.transform.position;

        float troopAttackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, troopAttackRange, TroopSide.Player, null, false);

        if (enemyInAttackRange != null)
            StateController.ActivateAttackState(target);
        else
        {
            Action finishAction = () => StateController.ActivateAttackState(target);

            const float distanceDelta = 0.15f;
            const float distanceModifier = 1 - distanceDelta;

            Vector3 direction = (targetPos - transform.position).normalized;
            targetPos -= direction * troopAttackRange * distanceModifier;

            StateController.ActivateMoveState(targetPos, finishAction);
        }
    }
}