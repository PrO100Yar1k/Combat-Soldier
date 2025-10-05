using System;
using UnityEngine;
using System.Collections;

public abstract class TroopAttackState : TroopBaseState
{
    protected event Action<IDamagable> OnActivateTroopAttack = default;

    protected Coroutine _reloadAttackCoroutine = default;
    protected Coroutine _attackCoroutine = default;

    protected int _remainingAttackWaves = default;

    protected TroopSide _enemyTroopSide = default;

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

    public TroopAttackState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {
        _remainingAttackWaves = _troopScriptable.CountAttackWaves;
    }

    public override void Start()
    {
        SubscribeToEvents();

        EnableStateIcon();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();

        DisableAttackCoroutine();
        ReloadAttackStarter();
    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/attack_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    public void ActivateAttack(IDamagable enemyDamagable)
        => OnActivateTroopAttack?.Invoke(enemyDamagable);

    private void TryToAttackEnemy(IDamagable enemyDamagable)
    {
        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyMonoBehaviour = RepositoryManager.instance.GetClosestEnemyInRange(troopPosition, attackRange, _enemyTroopSide, enemyDamagable, true);

        if (enemyMonoBehaviour == null)
            _switcherState.SwitchState<TroopDefaultState>();

        AttackEnemyCoroutineStarter(enemyMonoBehaviour);
    }

    #region Attack Coroutine Starter

    private void AttackEnemyCoroutineStarter(MonoBehaviour targetEnemy)
    {
        if (targetEnemy == null)
            return;

        DisableAttackCoroutine();
        EnableCoroutine(targetEnemy);
    }

    private void DisableAttackCoroutine()
    {
        if (_attackCoroutine == null)
            return;

        _troopController.StopCoroutine(_attackCoroutine);
        _attackCoroutine = null;
    }

    private void EnableCoroutine(MonoBehaviour enemyController)
    {
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemyCoroutine(enemyController));
    }

    #endregion

    #region Attack Coroutine Performance

    private IEnumerator AttackEnemyCoroutine(MonoBehaviour targetEnemy)
    {
        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        for ( ; _remainingAttackWaves > 0; _remainingAttackWaves--)
        {
            if (isEnemyStillAlive(targetEnemy) == false)
                break;

            if (isEnemyWithinAttackRange(targetEnemy) == false)
                break;

            BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
            bulletController.InitializeBullet(_troopController.transform.position, targetEnemy.transform.position);

            PlayerTroopController playerController = _troopController as PlayerTroopController;
            playerController?.ScreenCanvasUpdateReloadingBar(timeBetweenAttackWaves);

            yield return new WaitForSeconds(bulletController.GetBulletLifetime());

            IDamagable targetDamagable = targetEnemy as IDamagable;

            int attackDamage = _troopScriptable.AttackDamage;
            targetDamagable.TakeDamage(attackDamage);

            if (isEnemyStillAlive(targetEnemy) == false)
                break;

            IReactableForDamage enemyReactableForDamage = targetEnemy as IReactableForDamage;
            enemyReactableForDamage?.ReactionForTakingDamage(_troopController);

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        ReloadAttackStarter();

        AttackActionCompletion(targetEnemy);
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

    #region Extra Methods

    private void AttackActionCompletion(MonoBehaviour targetEnemy)
    {
        if (isEnemyStillAlive(targetEnemy) && isEnemyWithinAttackRange(targetEnemy))
            AttackEnemyCoroutineStarter(targetEnemy);
        else _switcherState.SwitchState<TroopDefaultState>();
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
        const float initialDelay = 0.25f;

        yield return new WaitForSeconds(initialDelay);

        int attackWavesCount = _troopScriptable.CountAttackWaves;

        float timeToCompleteReload = _troopScriptable.TimeToReloadAttack;
        float timeToReloadAttack = timeToCompleteReload / attackWavesCount * _remainingAttackWaves;

        PlayerTroopController playerController = _troopController as PlayerTroopController;
        playerController?.ScreenCanvasUpdateReloadingBar(timeToReloadAttack);

        for ( ; _remainingAttackWaves < attackWavesCount + 1; _remainingAttackWaves++)
        {
            yield return new WaitForSeconds(timeToReloadAttack / attackWavesCount);
        }
    }

    #endregion
}
