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

    public void Attack(IDamagable attackTarget)
    {
        _buildingController.StartCoroutine(AttackCoroutine(attackTarget));
    }

    private IEnumerator AttackCoroutine(IDamagable attackTarget)
    {
        while (true)
        {
            if (attackTarget == null)
                break;

            int damage = _buildingScriptable.Damage;

            attackTarget.TakeDamage(damage);

            yield return new WaitForEndOfFrame();
        }
    }
}
