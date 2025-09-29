using System.Collections;
using UnityEngine;

public class EnemyDefaultState : TroopDefaultState
{
    private Coroutine _movingBetweenPoinsCoroutine = default;
    private Coroutine _findUnitsCoroutine = default;

    public EnemyDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public override void Start()
    {
        if (_findUnitsCoroutine == null)
            _findUnitsCoroutine = _troopController.StartCoroutine(FindPlayerUnits());

        if (_movingBetweenPoinsCoroutine == null)
            _movingBetweenPoinsCoroutine = _troopController.StartCoroutine(MovingFromPointToPoint());

        EnableStateIcon();
    }

    public override void Stop()
    {
        if (_findUnitsCoroutine != null)
        {
            _troopController.StopCoroutine(_findUnitsCoroutine);
            _findUnitsCoroutine = null;
        }
    }

    private IEnumerator FindPlayerUnits()
    {
        IDamagable targetPriorityEnemy = null;
        TroopSide targetTroopSide = TroopSide.Player;

        float visibleRange = _troopScriptable.ViewRangeRadius;
        float attackRange = _troopScriptable.AttackRangeRadius;

        while (true)
        {
            const float delay = 1.0f;

            Vector3 currentPosition = _troopController.transform.position;

            MonoBehaviour closestEnemyInViewRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, visibleRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInViewRange != null)
            {
                EnemyTroopController enemyTroopController = _troopController as EnemyTroopController;

                IDamagable enemyDamagable = closestEnemyInViewRange as IDamagable;

                Vector3 targetPos = closestEnemyInViewRange.transform.position;

                enemyTroopController.MoveAndAttackEnemy(enemyDamagable, targetPos, attackRange);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator MovingFromPointToPoint()
    {
        yield return null;
    }

}
