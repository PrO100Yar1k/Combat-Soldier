using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private int _remainingAttackWaves = default;

    private Coroutine _attackCoroutine = default;

    //private bool _isAttack = false;

    public TroopAttackState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState)
    {
        // to do

        SetupDefaultCountAttackWaves();
    }

    public override void Start()
    {
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopStartedAttack += TryToAttackEnemy;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopStartedAttack -= TryToAttackEnemy;
    }

    private void TryToAttackEnemy(TroopController targetEnemy)
    {
        Debug.Log("Attack state entered!");

        Vector3 troopPosition = _troopController.transform.position;
        TroopSide enemyTroopSide = TroopSide.Enemy;

        TroopController enemyTroopController = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, _troopScriptable.attackRangeRadius, targetEnemy);

        if (enemyTroopController == null)   // !isEnemyInAttackRange()
            return;

        AttackCoroutineStarter(enemyTroopController);
    }

    private void AttackCoroutineStarter(TroopController targetEnemy)
    {
        if (_attackCoroutine != null)
        {
            _troopController.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
        else
        {
            _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(targetEnemy));
        }
    }

    private IEnumerator AttackEnemy(TroopController enemyController)
    {
        //_isAttack = true;

        yield return new WaitUntil(()=> _remainingAttackWaves == 0);

        float timeBetweenAttackWaves = _troopScriptable.timeBetweenAttackWaves;

        while (_remainingAttackWaves > 0)
        {
            enemyController.HPController.TakeDamage(_troopScriptable.attackDamage);

            Debug.Log($"Attacked with damage {_troopScriptable.attackDamage}; Wave - {_troopScriptable.countAttackWaves - _remainingAttackWaves}");

            _remainingAttackWaves--;

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        _troopController.StartCoroutine(ReloadAttack());

        //_isAttack = false;
    }

    private IEnumerator ReloadAttack()
    {
        float timeToReloadAttack = _troopScriptable.timeToReloadAttack;

        yield return new WaitForSeconds(timeToReloadAttack);

        SetupDefaultCountAttackWaves();
    }

    private void SetupDefaultCountAttackWaves()
        => _remainingAttackWaves = _troopScriptable.countAttackWaves;
}
