using System.Threading.Tasks;
using UnityEngine;

public class PlayerDefaultState : TroopDefaultState
{
    private const int _initialDelay = 1000; // in milliseconds

    public PlayerDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public override void Start()
    {
        CallCheckEnemyInAttackRange();
        EnableStateIcon();
    }

    public override void Stop()
    {

    }

    private void CallCheckEnemyInAttackRange()
        => Task.Delay(_initialDelay).ContinueWith(task => CheckEnemyInAttackRange());

    private void CheckEnemyInAttackRange()
    {
        Vector3 currentPosition = _troopController.transform.position;
        float attackRange = _troopScriptable.AttackRangeRadius;

        TroopSide targetTroopSide = TroopSide.Player;
        IDamagable targetPriorityEnemy = null;

        MonoBehaviour enemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false);

        if (enemyInAttackRange != null)
        {
            IDamagable enemyDamagable = enemyInAttackRange as IDamagable;

            _troopController.StateController.ActivateAttackState(enemyDamagable);
        }
    }
}
