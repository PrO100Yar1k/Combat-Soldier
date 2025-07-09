using System;
using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private event Action<IDamagable> OnActivateTroopAttack = default;

    private Coroutine _reloadAttackCoroutine = default;
    private Coroutine _attackCoroutine = default;

    private int _remainingAttackWaves = default;

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
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();

        DisableAttackCoroutine();
        ReloadAttackStarter();
    }

    public void ActivateTroopAttack(IDamagable enemyHPController)
        => OnActivateTroopAttack?.Invoke(enemyHPController);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/attack_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void TryToAttackEnemy(IDamagable enemyDamagable)
    {
        Vector3 troopPosition = _troopController.transform.position;
        TroopSide enemyTroopSide = TroopSide.Enemy;

        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyTroop = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, attackRange, enemyDamagable);

        if (enemyTroop == null)
            return;

        if (enemyTroop.TryGetComponent(out TroopController troopController))
        {
            troopController.StateController.ActivateDefenceState(); 
            AttackEnemyCoroutineStarter(troopController);
        }
        else if (enemyTroop.TryGetComponent(out BuildingController buildingController))
        {
            AttackEnemyCoroutineStarter(buildingController);
        }
        
    }

    #region Coroutine Starter

    private void AttackEnemyCoroutineStarter<T>(T targetEnemy) where T : MonoBehaviour, IDamagable
    {
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

    private IEnumerator AttackEnemy<T>(T enemyGenericTroop) where T : MonoBehaviour, IDamagable
    {
        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        for ( ; _remainingAttackWaves > 0; _remainingAttackWaves--)
        {
            if (isEnemyStillAlive(enemyGenericTroop) == false)
            {
                ReloadAttackStarter();
                break;
            }

            IResistable enemyResistable = enemyGenericTroop as IResistable;

            BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
            bulletController.InitializeBullet(_troopController.transform.position, enemyGenericTroop.transform.position);

            float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;
            GameEvents.instance.ReloadingTroop(timeBetweenAttackWaves);

            yield return new WaitForSeconds(bulletController.GetBulletLifetime());

            enemyGenericTroop.TakeDamage(_troopScriptable.AttackDamage);
            enemyResistable?.ActivateDefenseUnderAttack(_troopController.HPController, _troopController.transform.position);

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        ReloadAttackStarter();

        if (isEnemyStillAlive(enemyGenericTroop))
        {
            AttackEnemyCoroutineStarter(enemyGenericTroop);
        }
    }

    #endregion

    #region Helper Methods

    private bool isEnemyStillAlive<T>(T enemy) where T : MonoBehaviour, IDamagable
        => enemy != null;

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
        float timeToReloadAttack = _troopScriptable.TimeToReloadAttack;
        int attackWavesCount = _troopScriptable.CountAttackWaves;

        GameEvents.instance.ReloadingTroop(timeToReloadAttack);

        while (_remainingAttackWaves < attackWavesCount + 1)
        {
            yield return new WaitForSeconds(timeToReloadAttack / attackWavesCount);

            _remainingAttackWaves++;
        }
    }

    #endregion
}
