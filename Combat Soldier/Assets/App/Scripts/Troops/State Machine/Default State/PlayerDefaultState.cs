using System.Collections;
using UnityEngine;

public class PlayerDefaultState : TroopDefaultState
{
    public PlayerDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public override void Start()
    {
        CallCheckEnemyInAttackRange();

        EnableStateIcon();
    }

    public override void Stop()
    {

    }

    private void CallCheckEnemyInAttackRange()
        => _troopController.StartCoroutine(CheckEnemyOnceInAttackRange());

    private IEnumerator CheckEnemyOnceInAttackRange()
    {
        const float initialDelay = 0.2f;

        yield return new WaitForSeconds(initialDelay);

        Vector3 currentPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        TroopSide targetTroopSide = TroopSide.Enemy;
        IDamagable targetPriorityEnemy = null;

        MonoBehaviour enemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, true);

        if (enemyInAttackRange == null)
            yield break;

        _troopController.StateController.ActivateAttackState(enemyInAttackRange as IDamagable);
    }
}
