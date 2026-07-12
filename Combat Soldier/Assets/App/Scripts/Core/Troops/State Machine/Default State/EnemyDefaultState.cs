using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyDefaultState : TroopDefaultState
{
    private readonly Queue<Vector3> _patrollingPointsQueue = new Queue<Vector3>();

    private Coroutine _patrollingCoroutine = default;
    private Coroutine _findEnemyCoroutine = default;

    private const float minWaitingTime = 8.0f;
    private const float maxWaitingTime = 15.0f;

    private const float _enemyFindingDelay = 1.0f;
    private const float _reactionTime = 0.5f;

    public EnemyDefaultState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState,
        [Inject(Id = "Enemy Points")] Transform[] patrollingPointsList) : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {
        foreach (Transform targetPoint in patrollingPointsList)
            _patrollingPointsQueue.Enqueue(targetPoint.position);
    }

    public override void Start()
    {
        StartFindingEnemyCoroutine();
        StartPatrollingCoroutine();

        EnableStateIcon();
    }

    public override void Stop()
    {
        StopFindingEnemyCoroutine();
        StopPatrollingCoroutine();
    }

    #region Start & Stop Finding Enemy Coroutine

    private void StartFindingEnemyCoroutine()
    {
        if (_findEnemyCoroutine != null)
            StopFindingEnemyCoroutine();

        _findEnemyCoroutine = _troopController.StartCoroutine(FindingEnemyCoroutine());
    }

    private void StopFindingEnemyCoroutine()
    {
        if (_findEnemyCoroutine == null)
            return;

        _troopController.StopCoroutine(_findEnemyCoroutine);
        _findEnemyCoroutine = null;
    }

    #endregion

    #region Start & Stop Patrolling Coroutine

    private void StartPatrollingCoroutine()
    {
        if (_patrollingCoroutine != null)
            StopPatrollingCoroutine();

        _patrollingCoroutine = _troopController.StartCoroutine(PatrollingCoroutine());
    }

    private void StopPatrollingCoroutine()
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
        Vector3 targetPoint = _patrollingPointsQueue.Dequeue();
        _patrollingPointsQueue.Enqueue(targetPoint);

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
            Vector3 currentPosition = _troopController.transform.position;

            MonoBehaviour closestEnemyInAttackRange = _repositoryManager.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInAttackRange != null)
            {
                yield return new WaitForSeconds(_reactionTime);

                IDamagable enemyDamagable = closestEnemyInAttackRange as IDamagable;
                _troopController.StateController.ActivateAttackState(enemyDamagable);

                yield break;
            }

            MonoBehaviour closestEnemyInViewRange = _repositoryManager.GetClosestEnemyInRange(currentPosition, visibleRange, targetTroopSide, targetPriorityEnemy, false);

            if (closestEnemyInViewRange != null)
            {
                IDamagable enemyDamagable = closestEnemyInViewRange as IDamagable;
                Vector3 targetPos = closestEnemyInViewRange.transform.position;

                MoveToEnemyTarget(enemyDamagable, targetPos, attackRange);

                yield break;
            }

            yield return new WaitForSeconds(_enemyFindingDelay);
        }
    }

    private void MoveToEnemyTarget(IDamagable targetDamagable, Vector3 targetPos, float troopAttackRange) // to do extension method
    {
        const float distanceDelta = 0.15f;
        const float distanceModifier = 1 - distanceDelta;

        Vector3 currentPosition = _troopController.transform.position;

        Vector3 direction = (targetPos - currentPosition).normalized;
        targetPos -= direction * troopAttackRange * distanceModifier;

        _troopController.StateController.ActivateMoveState(targetPos);
    }

    #endregion
}
