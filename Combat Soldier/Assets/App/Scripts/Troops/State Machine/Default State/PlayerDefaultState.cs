using System.Collections;
using UnityEngine;

public class PlayerDefaultState : TroopDefaultState
{
    public PlayerDefaultState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }

    public override void Start()
    {
        CheckEnemyInAttackRangeStarter();
        EnableStateIcon();
    }

    public override void Stop()
    {

    }

    private void CheckEnemyInAttackRangeStarter()
        => _troopController.StartCoroutine(CheckEnemyOnceInAttackRange());

    private IEnumerator CheckEnemyOnceInAttackRange()
    {
        const float initialDelay = 0.2f;

        yield return new WaitForSeconds(initialDelay);

        Vector3 currentPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        TroopSide targetTroopSide = TroopSide.Enemy;
        IDamagable targetPriorityEnemy = null;

        MonoBehaviour enemyInAttackRange = _repositoryManager.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, true);

        if (enemyInAttackRange == null)
            yield break;

        _troopController.StateController.ActivateAttackState(enemyInAttackRange as IDamagable);
    }
}
