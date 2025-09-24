using UnityEngine;
using System.Collections;

public abstract class BaseBuildingAttack
{
    protected readonly BuildingController _buildingController = default;
    protected readonly BuildingScriptable _buildingScriptable = default;

    protected const float _checkTargetDelay = 1f;
    protected const float _reactionTime = 0.5f;

    public BaseBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable)
    {
        _buildingController = buildingController;
        _buildingScriptable = buildingScriptable;
    }

    public virtual IEnumerator CheckAttackTargetCoroutine()
    {
        Vector3 currentPosition = _buildingController.transform.position;
        float attackRange = _buildingScriptable.AttackRange;

        TroopSide targetTroopSide = TroopSide.Player;
        IDamagable targetPriorityEnemy = null;

        while (true)
        {
            IDamagable playerTroopController = GetTargetEnemy(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy);

            if (playerTroopController != null)
            {
                yield return _buildingController.StartCoroutine(AttackCoroutine(playerTroopController));
            }

            yield return new WaitForSeconds(_checkTargetDelay);
        }
    }

    protected void Attack(IDamagable attackTarget)
    {
        if (attackTarget == null)
            return;

        TroopController enemyTroopController = attackTarget as TroopController;

        int damage = _buildingScriptable.Damage;
        attackTarget.TakeDamage(damage);

        enemyTroopController?.ActivateDefenseUnderAttack(_buildingController, _buildingController.transform.position);
    }

    protected bool isTroopStillAlive(IDamagable troopIDamagable, out Transform troopTransform)
    {
        UnityEngine.Object troopObject = troopIDamagable as UnityEngine.Object;

        MonoBehaviour troopMonobehaviour = troopObject as MonoBehaviour;
        troopTransform = troopMonobehaviour != null ? troopMonobehaviour.transform : null;

        if (troopObject == null)
            return false;

        if (troopTransform == null)
            return false;

        return true;
    }

    protected abstract IEnumerator AttackCoroutine(IDamagable troopIDamagable);

    protected abstract IDamagable GetTargetEnemy(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy);
}

