using UnityEngine;
using System.Collections;

public class DefaultBuildingAttack : IAttackable
{
    private readonly BuildingController _buildingController = default;
    private readonly BuildingScriptable _buildingScriptable = default;

    public DefaultBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable)
    {
        _buildingController = buildingController;
        _buildingScriptable = buildingScriptable;
    }

    public IEnumerator CheckAttackTargetCoroutine()
    {
        IDamagable targetPriorityEnemy = null;
        TroopSide targetTroopSide = TroopSide.Player;

        float attackRange = _buildingScriptable.AttackRange;
        Vector3 currentPosition = _buildingController.transform.position;

        while (true)
        {
            IDamagable playerTroopController = TroopGeneralManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy) as IDamagable;

            if (playerTroopController != null)
            {
                yield return _buildingController.StartCoroutine(_buildingController.AttackPlayerCoroutine(playerTroopController));
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void Attack(IDamagable attackTarget)
    {
        int damage = _buildingScriptable.Damage;
        attackTarget.TakeDamage(damage);
    }
}
