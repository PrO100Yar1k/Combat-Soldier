using System;
using UnityEngine;
using System.Collections;

public class TroopAttackState : TroopBaseState
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

    public void ActivateAttack(IDamagable enemyDamagable)
        => OnActivateTroopAttack?.Invoke(enemyDamagable);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/attack_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void TryToAttackEnemy(IDamagable enemyDamagable)
    {
        Vector3 troopPosition = _troopController.transform.position;

        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyTroop = RepositoryManager.instance.GetClosestEnemyInRange(troopPosition, attackRange, _enemyTroopSide, enemyDamagable, true);

        if (enemyTroop == null)
            return;

        if (enemyTroop.TryGetComponent(out TroopController troopController)) //
        {
            // _switcherState.SwitchState<TroopDefenseState>(); 
            AttackEnemyCoroutineStarter(troopController);
        }
        else if (enemyTroop.TryGetComponent(out BuildingController buildingController))
        {
            AttackEnemyCoroutineStarter(buildingController); //
        }

        else _switcherState.SwitchState<TroopDefaultState>();
    }

    #region Coroutine Starter

    private void AttackEnemyCoroutineStarter<T>(T targetEnemy) where T : MonoBehaviour, IDamagable
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

    private void EnableCoroutine<T>(T enemyController) where T : MonoBehaviour, IDamagable
    {
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(enemyController));
    }

    #endregion

    #region Attack Coroutine

    private IEnumerator AttackEnemy<T>(T enemyTroop) where T : MonoBehaviour, IDamagable
    {
        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        for ( ; _remainingAttackWaves > 0; _remainingAttackWaves--)
        {
            if (isEnemyStillAlive(enemyTroop) == false)
                break;

            Vector3 currentPosition = _troopController.transform.position;
            Vector3 enemyPosition = enemyTroop.transform.position;

            float attackRange = _troopScriptable.AttackRangeRadius;

            if (Vector3.Distance(currentPosition, enemyPosition) > attackRange)
                break;

            IReactableForDamage enemyReactableForDamage = enemyTroop as IReactableForDamage;

            BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
            bulletController.InitializeBullet(_troopController.transform.position, enemyTroop.transform.position);

            float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

            PlayerTroopController playerController = _troopController as PlayerTroopController;
            playerController?.ScreenCanvasUpdateReloadingBar(timeBetweenAttackWaves);

            yield return new WaitForSeconds(bulletController.GetBulletLifetime());

            enemyTroop.TakeDamage(_troopScriptable.AttackDamage);

            if (isEnemyStillAlive(enemyTroop) == false)
                break;

            enemyReactableForDamage?.ReactionForTakingDamage(_troopController);

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        ReloadAttackStarter();

        FinishAttackCoroutineAction(enemyTroop);
    }

    #endregion

    #region Helper Methods

    private bool isEnemyStillAlive<T>(T enemy) where T : MonoBehaviour, IDamagable
        => enemy != null;

    //

    #endregion

    #region Extra Methods

    private void FinishAttackCoroutineAction<T>(T enemyTroop) where T : MonoBehaviour, IDamagable
    {
        if (isEnemyStillAlive(enemyTroop) && Vector3.Distance(_troopController.transform.position, enemyTroop.transform.position) <= _troopScriptable.AttackRangeRadius) AttackEnemyCoroutineStarter(enemyTroop);
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
        float timeToCompleteReload = _troopScriptable.TimeToReloadAttack; //

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
