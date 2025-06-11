using System;
using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private event Action<TroopController> OnActivateTroopAttack = default;

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

    public TroopAttackState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
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

    public void ActivateTroopAttack(TroopController enemyController)
        => OnActivateTroopAttack?.Invoke(enemyController);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/attack_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void TryToAttackEnemy(TroopController enemyController)
    {
        Vector3 troopPosition = _troopController.transform.position;
        TroopSide enemyTroopSide = TroopSide.Enemy;

        float attackRange = _troopScriptable.AttackRangeRadius;

        TroopController enemyTroopController = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, attackRange, enemyController);

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

    private void EnableCoroutine(TroopController targetEnemy)
    {
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(targetEnemy));
    }

    #endregion

    #region Attack Coroutine

    private IEnumerator AttackEnemy(TroopController enemyController)
    {
        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        while (_remainingAttackWaves > 0)
        {
            if (enemyController == null)
                break;

            enemyController.HPController.TakeDamage(_troopScriptable.AttackDamage);

            enemyController.StateController.ActivateDefenseUnderAttack(_troopController);

            Debug.Log($"Attacked with damage {_troopScriptable.AttackDamage}; Wave - {_troopScriptable.CountAttackWaves - _remainingAttackWaves}");

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
