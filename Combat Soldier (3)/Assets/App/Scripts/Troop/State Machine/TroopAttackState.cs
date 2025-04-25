using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private int _countAttackWaves = default;

    private bool _isAttack = false;

    public TroopAttackState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState)
    {
        //SetupDefaultCountAttackWaves();
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

        _troopController.StartCoroutine(AttackEnemy(enemyController));
    }

    private IEnumerator AttackEnemy(TroopController enemyController)
    {
        _isAttack = true;

        float timeBetweenAttack = _troopScriptable.timeToNextAttack;

        while (_countAttackWaves > 0)
        {
            enemyController.HPController.TakeDamage(_troopScriptable.attackDamage);

            _countAttackWaves--;

            Debug.Log($"Attacked with damage {_troopScriptable.attackDamage}; Wave - {_troopScriptable.countAttackWaves - _countAttackWaves}");

            yield return new WaitForSeconds(timeBetweenAttack);
        }

        _isAttack = false;
    }

    private bool isEnemyInAttackRange()
    {
        return true;

        //if ()
    }

    private void SetupDefaultCountAttackWaves()
        => _countAttackWaves = _troopScriptable.countAttackWaves;
}
