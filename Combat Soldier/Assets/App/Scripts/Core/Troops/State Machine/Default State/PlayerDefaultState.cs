using UnityEngine;
using Assets.App.Scripts;
using System.Collections;
using Assets.App.Scripts.Core.Canvases;

public class PlayerDefaultState : TroopDefaultState
{
    public PlayerDefaultState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    public override void OnStart()
    {
        PlayStateAnimation();
        CheckEnemyInAttackRange();
    }

    public override void OnStop()
    {

    }

    private void CheckEnemyInAttackRange()
    {
        _troopController.StartCoroutine(CheckEnemyOnceInAttackRange());
    }

    private IEnumerator CheckEnemyOnceInAttackRange(Faction targetFaction = Faction.Enemies, IDamagable targetPriorityEnemy = null)
    {
        const float initialDelay = 0.25f;

        yield return new WaitForSeconds(initialDelay);

        Vector3 currentPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyInAttackRange = _targetSearchService.GetClosestEnemyInRange(currentPosition, attackRange, targetFaction, targetPriorityEnemy, true);

        if (enemyInAttackRange == null)
            yield break;

        Vector3 targetLookAtPosition = new Vector3(enemyInAttackRange.transform.position.x, _troopController.transform.position.y, enemyInAttackRange.transform.position.z);
        _troopController.transform.LookAt(targetLookAtPosition);

        _troopController.StateController.ActivateAttackState(enemyInAttackRange as IDamagable);
    }
}
