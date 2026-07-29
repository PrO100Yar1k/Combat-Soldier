using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;
using Assets.App.Scripts.Infrastructure.Others;
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

    private const float _enemyFindingDelay = 0.5f;
    private const float _reactionTime = 0.3f;

    public EnemyDefaultState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState,
        [Inject(Id = "Enemy Points")] Transform[] patrollingPointsList, ITroopAnimator animatorController) : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {
        foreach (Transform targetPoint in patrollingPointsList)
            _patrollingPointsQueue.Enqueue(targetPoint.position);
    }

    public override void OnStart()
    {
        PlayStateAnimation();

        StartFindingEnemyCoroutine();
        StartPatrollingCoroutine();
    }

    public override void OnStop()
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

    private IEnumerator FindingEnemyCoroutine(IDamagable targetPriorityEnemy = null, Faction targetFaction = Faction.Allies)
    {
        float visibleRange = _troopScriptable.ViewRangeRadius;
        float attackRange = _troopScriptable.AttackRangeRadius;

        while (true)
        {
            Vector3 currentPosition = _troopController.transform.position;

            MonoBehaviour closestEnemyInAttackRange = _targetSearchService.GetClosestEnemyInRange(currentPosition, attackRange, targetFaction, targetPriorityEnemy, false);

            if (closestEnemyInAttackRange != null)
            {
                yield return new WaitForSeconds(_reactionTime);

                Vector3 targetLookAtPosition = new Vector3(closestEnemyInAttackRange.transform.position.x, _troopController.transform.position.y, closestEnemyInAttackRange.transform.position.z);
                _troopController.transform.LookAt(targetLookAtPosition);

                IDamagable enemyDamagable = closestEnemyInAttackRange as IDamagable;
                _troopController.StateController.ActivateAttackState(enemyDamagable);

                yield break;
            }

            MonoBehaviour closestEnemyInViewRange = _targetSearchService.GetClosestEnemyInRange(currentPosition, visibleRange, targetFaction, targetPriorityEnemy, false);

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

    protected void MoveToEnemyTarget(IDamagable targetDamagable, Vector3 targetPos, float troopAttackRange)
    {
        Vector3 currentPosition = _troopController.transform.position;
        Vector3 destination = CombatMath.GetAttackDestination(currentPosition, targetPos, troopAttackRange);

        _troopController.StateController.ActivateMoveState(destination);
    }

    #endregion
}
