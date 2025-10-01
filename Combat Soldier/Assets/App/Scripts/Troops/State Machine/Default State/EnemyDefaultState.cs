using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyDefaultState : TroopDefaultState
{
    private readonly Queue<Vector3> _targetPointsQueue = new Queue<Vector3>();

    private Coroutine _patrollingCoroutine = default;
    private Coroutine _findEnemyCoroutine = default;

    private const float minWaitingTime = 8;
    private const float maxWaitingTime = 15;

    public EnemyDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, Transform[] targetPointsList) : base(troopController, screenCanvasController, switcherState)
    {
        foreach (Transform targetPoint in targetPointsList)
            _targetPointsQueue.Enqueue(targetPoint.position);
    }

    public override void Start()
    {
        StartFindingEnemy();
        StartPatrolling();

        EnableStateIcon();
    }

    public override void Stop()
    {
        StopFindingEnemy();
        StopPatrolling();
    }

    #region Start & Stop Finding Enemy Coroutine

    private void StartFindingEnemy()
    {
        if (_findEnemyCoroutine != null)
            return;

        _findEnemyCoroutine = _troopController.StartCoroutine(FindingEnemyCoroutine());
    }

    private void StopFindingEnemy()
    {
        if (_findEnemyCoroutine == null)
            return;

        _troopController.StopCoroutine(_findEnemyCoroutine);
        _findEnemyCoroutine = null;
    }

    #endregion

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

        _troopController.StateController.ActivateMoveState(targetPos);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPoint = _targetPointsQueue.Dequeue();
        _targetPointsQueue.Enqueue(targetPoint);

        return targetPoint;
    }

    #endregion

    #region Finding Enemy Cycle

    private IEnumerator FindingEnemyCoroutine() // finding player's unit in visible range while default state
    {
        IDamagable targetPriorityEnemy = null;
        TroopSide targetTroopSide = TroopSide.Player;

        float visibleRange = _troopScriptable.ViewRangeRadius;
        float attackRange = _troopScriptable.AttackRangeRadius;

        while (true)
        {
            const float delay = 1.0f;

            Vector3 currentPosition = _troopController.transform.position;

            MonoBehaviour closestEnemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInAttackRange != null) //
            {
                EnemyTroopController enemyTroopController = _troopController as EnemyTroopController;

                IDamagable enemyDamagable = closestEnemyInAttackRange as IDamagable;

                Vector3 targetPos = closestEnemyInAttackRange.transform.position;

                yield return new WaitForSeconds(0.25f);

                enemyTroopController.StateController.ActivateAttackState(enemyDamagable);

                yield break; //
            }

            MonoBehaviour closestEnemyInViewRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, visibleRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInViewRange != null) //
            {
                EnemyTroopController enemyTroopController = _troopController as EnemyTroopController;

                IDamagable enemyDamagable = closestEnemyInViewRange as IDamagable;

                Vector3 targetPos = closestEnemyInViewRange.transform.position;

                enemyTroopController.MoveToEnemyTarget(enemyDamagable, targetPos, attackRange);

                yield break; //
            }

            yield return new WaitForSeconds(delay);
        }
    }

    #endregion
}
