using Assets.App.Scripts;
using System.Collections;
using UnityEngine;

public class PlayerDefaultState : TroopDefaultState
{
    public PlayerDefaultState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    public override void OnStart()
    {
        CheckEnemyInAttackRangeStarter();
    }

    public override void OnStop()
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

        Faction targetTroopSide = Faction.Enemies;
        IDamagable targetPriorityEnemy = null;

        MonoBehaviour enemyInAttackRange = _repositoryManager.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, true);

        if (enemyInAttackRange == null)
            yield break;

        _troopController.StateController.ActivateAttackState(enemyInAttackRange as IDamagable);
    }
}
