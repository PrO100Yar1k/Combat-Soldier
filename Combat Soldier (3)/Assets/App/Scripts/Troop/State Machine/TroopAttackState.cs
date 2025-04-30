using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private int _remainingAttackWaves = default;

    private Coroutine _attackCoroutine = default;

    #region Events

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopAttackEnemy += TryToAttackEnemy;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopAttackEnemy -= TryToAttackEnemy;
    }

    #endregion

    public TroopAttackState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState)
    {
        SetupDefaultCountAttackWaves();
    }

    public override void Start()
    {
        SubscribeToEvents();
    }

    public override void Stop()
    {
        DisableCoroutine();
        ReloadAttackStarter();

        UnSubscribeFromEvents();
    }

    private void TryToAttackEnemy(TroopController targetEnemy)
    {
        Vector3 troopPosition = _troopController.transform.position;
        TroopSide enemyTroopSide = TroopSide.Enemy;

        float attackRange = _troopScriptable.AttackRangeRadius;

        TroopController enemyTroopController = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, attackRange, targetEnemy);

        if (enemyTroopController == null)
            return;

        enemyTroopController.StateController.ActivateDefenceState(); // maybe change it to event ?

        AttackEnemyCoroutineStarter(enemyTroopController);
    }

    #region Coroutine Starter

    private void AttackEnemyCoroutineStarter(TroopController targetEnemy)
    {
        DisableCoroutine();

        EnableCoroutine(targetEnemy);
    }

    private void DisableCoroutine()
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
        _troopController.StartCoroutine(ReloadAttack());
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
