using Assets.App.Scripts;
using System.Collections;
using UnityEngine;
using System;

public abstract class TroopAttackState : TroopBaseState
{
    protected event Action<IDamagable> OnActivateTroopAttack = default;

    protected Coroutine _reloadAttackCoroutine = default;
    protected Coroutine _attackCoroutine = default;

    protected MonoBehaviour _currentTargetEnemy = default;

    protected Faction _enemyTroopSide = default;
    protected int _remainingAttackWaves = default;
    protected float _lastAttackTime = default;

    protected bool _isReloading = false;

    protected override string StateIconLocation
        => "State Icons/Attack-State-Icon";

    protected TroopAttackState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {
        _remainingAttackWaves = _troopScriptable.CountAttackWaves;
    }

    #region Events

    protected override void SubscribeToEvents()
    {
        OnActivateTroopAttack += TryToAttackEnemy;
    }

    protected override void UnSubscribeFromEvents()
    {
        OnActivateTroopAttack -= TryToAttackEnemy;
    }

    #endregion

    public override void OnStart()
    {
        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        if (Time.time - _lastAttackTime > timeBetweenAttackWaves)
        {
            _lastAttackTime = Time.time - timeBetweenAttackWaves;
        }
    }

    public override void OnStop()
    {
        DisableAttackCoroutine();
        ReloadAttackStarter();
        _isReloading = false;
    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayAttack();
    }

    public void ActivateAttack(IDamagable enemyDamagable)
    {
        OnActivateTroopAttack?.Invoke(enemyDamagable);
    }

    private void TryToAttackEnemy(IDamagable enemyDamagable)
    {
        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyMonoBehaviour = _repositoryManager.GetClosestEnemyInRange(troopPosition, attackRange, _enemyTroopSide, enemyDamagable, true);

        if (enemyMonoBehaviour == null)
            _switcherState.SwitchState<TroopDefaultState>();

        AttackEnemyCoroutineStarter(enemyMonoBehaviour);
    }

    #region Attack Coroutine Starter

    private void AttackEnemyCoroutineStarter(MonoBehaviour targetEnemy)
    {
        if (targetEnemy == null)
            return;

        if (_currentTargetEnemy == targetEnemy) //_attackCoroutine != null &&
            return;

        DisableAttackCoroutine();

        _currentTargetEnemy = targetEnemy;
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemyCoroutine(targetEnemy));
    }

    private void DisableAttackCoroutine()
    {
        if (_attackCoroutine == null)
            return;

        _troopController.StopCoroutine(_attackCoroutine);
        _currentTargetEnemy = null;
        _attackCoroutine = null;
    }

    #endregion

    #region Attack Coroutine Performance

    private IEnumerator AttackEnemyCoroutine(MonoBehaviour targetEnemy)
    {
        yield return new WaitUntil(()=> !_isReloading && _remainingAttackWaves > 0);

        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        while (_remainingAttackWaves > 0 && !_isReloading)
        {
            float timeSinceLastAttack = Time.time - _lastAttackTime;

            //if (timeSinceLastAttack < timeBetweenAttackWaves)
            //{
            //    yield return new WaitForSeconds(timeBetweenAttackWaves - timeSinceLastAttack);
            //}

            if (isEnemyStillAlive(targetEnemy) == false)
                break;

            if (isEnemyWithinAttackRange(targetEnemy) == false)
                break;

            _lastAttackTime = Time.time;
            _remainingAttackWaves--;

            PlayStateAnimation();

            Vector3 initialBulletPosition = _troopController.BulletInitialPoint.position;
            Vector3 targetBulletPosition = new Vector3(targetEnemy.transform.position.x, _troopController.BulletInitialPoint.position.y, targetEnemy.transform.position.z);

            BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
            bulletController.InitializeBullet(initialBulletPosition, targetBulletPosition);

            PlayerTroopController playerController = _troopController as PlayerTroopController;
            playerController?.UpdateReloadingBar(timeBetweenAttackWaves);

            float bulletLifetime = bulletController.GetBulletLifetime();
            yield return new WaitForSeconds(bulletLifetime);

            int attackDamage = _troopScriptable.AttackDamage;

            IDamagable targetDamagable = targetEnemy as IDamagable;
            targetDamagable?.TakeDamage(attackDamage);

            if (isEnemyStillAlive(targetEnemy) == false)
                break;

            IReactableForDamage enemyReactableForDamage = targetEnemy as IReactableForDamage;
            enemyReactableForDamage?.ReactionForTakingDamage(_troopController);

            yield return new WaitForSeconds(timeBetweenAttackWaves - bulletLifetime);
        }

        if (_remainingAttackWaves <= 0)
        {
            ReloadAttackStarter();
        }
        CheckForAttackStateCompletion();
    }

    private void CheckForAttackStateCompletion()
    {
        if (!isEnemyStillAlive(_currentTargetEnemy) || !isEnemyWithinAttackRange(_currentTargetEnemy))
            _switcherState.SwitchState<TroopDefaultState>();
    }

    #endregion

    #region Helper Methods

    private bool isEnemyStillAlive(MonoBehaviour targetEnemy)
        => targetEnemy != null;

    private bool isEnemyWithinAttackRange(MonoBehaviour targetEnemy)
    {
        Vector3 currentPosition = _troopController.transform.position;
        Vector3 enemyPosition = targetEnemy.transform.position;

        float attackRange = _troopScriptable.AttackRangeRadius;

        return Vector3.Distance(currentPosition, enemyPosition) <= attackRange;
    }

    #endregion

    #region Reload Attack

    private void ReloadAttackStarter()
    {
        if (_reloadAttackCoroutine != null)
        {
            _troopController.StopCoroutine(_reloadAttackCoroutine);
            _reloadAttackCoroutine = null;
        }

        _reloadAttackCoroutine = _troopController.StartCoroutine(ReloadAttack());
    }

    private IEnumerator ReloadAttack()
    {
        _isReloading = true;

        const float initialDelay = 0.25f;

        yield return new WaitForSeconds(initialDelay);

        int attackWavesCount = _troopScriptable.CountAttackWaves;

        float timeToCompleteReload = _troopScriptable.TimeToReloadAttack;
        float timeToReloadAttack = timeToCompleteReload / attackWavesCount;

        PlayerTroopController playerController = _troopController as PlayerTroopController;
        playerController?.UpdateReloadingBar(timeToCompleteReload);

        for ( ; _remainingAttackWaves < attackWavesCount; _remainingAttackWaves++)
        {
            yield return new WaitForSeconds(timeToReloadAttack);
        }

        _isReloading = false;

        CheckEnemies();
    }

    private void CheckEnemies()
    {
        MonoBehaviour unit = _currentTargetEnemy;

        if (isEnemyStillAlive(_currentTargetEnemy) && isEnemyWithinAttackRange(_currentTargetEnemy))
        {
            _currentTargetEnemy = null;
            AttackEnemyCoroutineStarter(unit);
        }
    }

    #endregion
}
