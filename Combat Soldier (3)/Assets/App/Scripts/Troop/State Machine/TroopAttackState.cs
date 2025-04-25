using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private int _remainingAttackWaves = default;

    private Coroutine _attackCoroutine = default;

    //private bool _isAttack = false;

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
        UnSubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        //GameEvents.instance.OnTroopStartedMovement += SetWaypoint;
    }

    private void UnSubscribeFromEvents()
    {
        //GameEvents.instance.OnTroopStartedMovement -= SetWaypoint;
    }

    private void TryToAttackEnemy(TroopController enemyController)
    {
        if (!isEnemyInAttackRange())
            return;

        AttackCoroutineStarter(enemyController);
    }

    private void AttackCoroutineStarter(TroopController enemyController)
    {
        if (_attackCoroutine != null)
        {
            _troopController.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
        else
        {
            _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(enemyController));
        }
    }

    private IEnumerator AttackEnemy(TroopController enemyController)
    {
        //_isAttack = true;

        float timeBetweenAttack = _troopScriptable.timeToNextAttack;

        while (_remainingAttackWaves > 0)
        {
            enemyController.HPController.TakeDamage(_troopScriptable.attackDamage);

            _remainingAttackWaves--;

            Debug.Log($"Attacked with damage {_troopScriptable.attackDamage}; Wave - {_troopScriptable.countAttackWaves - _remainingAttackWaves}");

            yield return new WaitForSeconds(timeBetweenAttack);
        }

        //_isAttack = false;
    }

    private bool isEnemyInAttackRange()
    {
        return true;

        //if ()
    }

    private void SetupDefaultCountAttackWaves()
        => _remainingAttackWaves = _troopScriptable.countAttackWaves;
}
