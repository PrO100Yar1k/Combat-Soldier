using System.Collections;
using System.Linq;
using UnityEngine;

public class ThirdBuildingAttack : BaseBuildingAttack
{
    private const int _maxAttackUnitCount = 10;

    public ThirdBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable) : base(buildingController, buildingScriptable)
    {

    }

    protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
    {
        float attackRange = _buildingScriptable.AttackRange;
        float reloadingTime = _buildingScriptable.ReloadingTime;

        yield return new WaitForSeconds(_reactionTime);

        while (true)
        {
            for (int i = 0; i < IDamagableTroopList.Length; i++)
            {
                IDamagable troopIDamagable = IDamagableTroopList[i];

                if (isTroopStillAlive(troopIDamagable, out Transform troopTransform) == false)
                    yield break;

                Vector3 buildingPosition = _buildingController.transform.position;
                Vector3 troopPosition = troopTransform.position;

                if (Vector3.Distance(buildingPosition, troopPosition) > attackRange)
                    yield break;

                Attack(troopIDamagable);
            }

            yield return new WaitForSeconds(reloadingTime);
        }
    }

    protected override IDamagable[] GetTargetEnemy(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy)
    {
        IDamagable[] IDamagableList = RepositoryManager.instance.GetEnemyListInRange(currentPosition, attackRange, targetTroopSide)
            ?.Take(_maxAttackUnitCount).ToArray();

        return IDamagableList;
    }
}
