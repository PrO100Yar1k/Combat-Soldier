using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyDefaultState : TroopDefaultState
{
    private readonly Queue<Vector3> _targetPointsQueue = new Queue<Vector3>();

    private Coroutine _patrollingCoroutine = default;
    private Coroutine _findUnitsCoroutine = default;

    private const float minWaitingTime = 5;
    private const float maxWaitingTime = 10;

    public EnemyDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, Transform[] targetPointsList) : base(troopController, screenCanvasController, switcherState)
    {
        foreach (Transform targetPoint in targetPointsList)
            _targetPointsQueue.Enqueue(targetPoint.position);
    }

    public override void Start()
    {
        if (_findUnitsCoroutine == null)
            _findUnitsCoroutine = _troopController.StartCoroutine(FindPlayerUnits());

        StartPatrolling();

        EnableStateIcon();
    }

    public override void Stop()
    {
        if (_findUnitsCoroutine != null)
        {
            _troopController.StopCoroutine(_findUnitsCoroutine);
            _findUnitsCoroutine = null;
        }

        StopPatrolling();
    }

    #region Start & Stop Patrolling Coroutine

    private void StartPatrolling()
    {
        if (_patrollingCoroutine != null)
            StopPatrolling();

        _patrollingCoroutine = _troopController.StartCoroutine(PatrollingCoroutine());
    }

    private void StopPatrolling()
    {
        if (_patrollingCoroutine == null)
            return;

        _troopController.StopCoroutine(_patrollingCoroutine);
        _patrollingCoroutine = null;
    }

    #endregion

    #region Patrolling Cycle

    private IEnumerator PatrollingCoroutine()
    {
        float waitingTime = Random.Range(minWaitingTime, maxWaitingTime);

        yield return new WaitForSeconds(waitingTime);

        Vector3 targetPos = GetTargetPosition();

        _troopController.StateController.ActivateMoveState(targetPos, null);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPoint = _targetPointsQueue.Dequeue();
        _targetPointsQueue.Enqueue(targetPoint);

        return targetPoint;
    }

    #endregion

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
}
