using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTroopController : TroopController, IReactableForDamage
{
    [SerializeField] private MeshRenderer _enemyMeshRendererModel = default;

    [SerializeField] private List<Transform> targetPointsList = new List<Transform>();

    public TroopModelController TroopModelController { get; private set; }

    protected override void InitializeTroop()
    {
        StateController = new EnemyTroopStateController(this, _screenCanvasController, targetPointsList.ToArray());
        TroopModelController = new TroopModelController(this, gameObject, _enemyMeshRendererModel);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);
    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable // ?
    {
        if (target == null || StateController.CheckStateForActivity<TroopAttackState>())
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPos = target.transform.position;

        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, TroopSide.Player, null, false);

        if (enemyInAttackRange != null)
        {
            StateController.ActivateDefenseUnderAttack(target, targetPos);
        }
        else
        {
            MoveAndAttackEnemy(target, targetPos, attackRange);
        }
    }

    public void MoveAndAttackEnemy(IDamagable targetDamagable, Vector3 targetPos, float troopAttackRange) //
    {
        Action finishAction = () => StateController.ActivateAttackState(targetDamagable);

        const float distanceDelta = 0.15f;
        const float distanceModifier = 1 - distanceDelta;

        Vector3 direction = (targetPos - transform.position).normalized;
        targetPos -= direction * troopAttackRange * distanceModifier;

        StateController.ActivateMoveState(targetPos, finishAction);
    }
}