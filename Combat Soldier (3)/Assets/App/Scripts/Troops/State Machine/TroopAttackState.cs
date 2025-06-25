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

    private void SubscribeToEvents()
    {
        OnActivateTroopAttack += TryToAttackEnemy;
    }

    private void UnSubscribeFromEvents()
    {
        OnActivateTroopAttack -= TryToAttackEnemy;
    }

    #endregion

    public TroopAttackState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {
        SetupDefaultCountAttackWaves();
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

        TroopController enemyTroopController = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, attackRange, enemyDamagable);

        if (enemyTroopController == null)
            return;

        enemyTroopController.StateController.ActivateDefenceState(); 

        AttackEnemyCoroutineStarter(enemyTroopController);
    }

    #region Coroutine Starter

    private void AttackEnemyCoroutineStarter(TroopController targetEnemy)
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

    private void EnableCoroutine(TroopController enemyController)
    {
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(enemyController));
    }

    #endregion

    #region Attack Coroutine

    private IEnumerator AttackEnemy(TroopController enemyTroopController)
    {
        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        while (_remainingAttackWaves > 0)
        {
            if (enemyTroopController == null)
                break;

            HPController enemyHPController = enemyTroopController.HPController;

            enemyHPController.TakeDamage(_troopScriptable.AttackDamage);

            enemyHPController.ActivateDefenseUnderAttack(_troopController.HPController);

            Debug.Log($"Attacked {enemyHPController.HPControllerName}; Wave - {_troopScriptable.CountAttackWaves - _remainingAttackWaves + 1}");

            _remainingAttackWaves--;

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        ReloadAttackStarter();
    }

    #endregion

    #region Reload Attack

    private void ReloadAttackStarter()
    {
        if (_reloadAttackCoroutine != null)
            return;

        _reloadAttackCoroutine = _troopController.StartCoroutine(ReloadAttack());
    }

    private IEnumerator ReloadAttack()
    {
        float timeToReloadAttack = _troopScriptable.TimeToReloadAttack;

        yield return new WaitForSeconds(timeToReloadAttack);

        SetupDefaultCountAttackWaves();
    }

    private void SetupDefaultCountAttackWaves()
        => _remainingAttackWaves = _troopScriptable.CountAttackWaves;

    #endregion
}
