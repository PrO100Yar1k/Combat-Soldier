using UnityEngine;
using System.Collections;

public class FirstBuildingAttack : BaseBuildingAttack
{
    public FirstBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable) : base(buildingController, buildingScriptable)
    {

    }

    protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
    {
        //if (IDamagableTroopList[0] == null)
        //    yield break;

        IDamagable troopIDamagable = IDamagableTroopList[0];

        float attackRange = _buildingScriptable.AttackRange;
        float reloadingTime = _buildingScriptable.ReloadingTime;

        yield return new WaitForSeconds(_reactionTime);

        while (true)
        {
            Transform troopTransform = default;

            if (isTroopStillAlive(troopIDamagable, out troopTransform) == false)
                yield break;

            Vector3 buildingPosition = _buildingController.transform.position;
            Vector3 troopPosition = troopTransform.position;

            if (Vector3.Distance(buildingPosition, troopPosition) > attackRange)
                yield break;

            Attack(troopIDamagable);

            yield return new WaitForSeconds(reloadingTime);
        }
    }

    protected override IDamagable[] GetTargetEnemy(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy)
    {
        IDamagable IDamagableTroop = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false) as IDamagable;

        return new IDamagable[] { IDamagableTroop };
    }
}
