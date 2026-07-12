using System.Collections;
using System.Linq;
using UnityEngine;

public class ThirdBuildingAttack : BaseBuildingAttack
{
    private const int _maxAttackUnitCount = 10;

    public ThirdBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable, RepositoryManager repositoryManager)
        : base(buildingController, buildingScriptable, repositoryManager)
    {
        // damage all enemies (above _maxAttackUnitCount, but could make infinite enemy count too) in the attack range based on usual reload
    }

    protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
    {
        float attackRange = _buildingScriptable.AttackRange;
        float reloadingTime = _buildingScriptable.ReloadingTime;

        Faction targetTroopSide = Faction.Allies;
        IDamagable targetPriorityEnemy = null;

        Vector3 buildingPosition = _buildingController.transform.position;

        yield return new WaitForSeconds(_reactionTime);

        while (true)
        {
            IDamagableTroopList = GetEnemyTargets(buildingPosition, attackRange, targetTroopSide, targetPriorityEnemy);

            if (IDamagableTroopList == null || IDamagableTroopList.Length == 0)
                yield break;

            for (int i = 0; i < IDamagableTroopList.Length; i++)
            {
                IDamagable troopIDamagable = IDamagableTroopList[i];

                if (isTroopStillAlive(troopIDamagable, out Transform troopTransform) == false)
                    yield break;

                Vector3 troopPosition = troopTransform.position;

                if (Vector3.Distance(buildingPosition, troopPosition) > attackRange)
                    yield break;

                Attack(troopIDamagable);
            }

            yield return new WaitForSeconds(reloadingTime);
        }
    }

    protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy)
    {
        return _repositoryManager.GetEnemyListInRange(currentPosition, attackRange, targetTroopSide)
            ?.Take(_maxAttackUnitCount).ToArray();
    }
}
