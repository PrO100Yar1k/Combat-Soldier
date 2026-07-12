using UnityEngine;
using System.Collections;

public class SecondBuildingAttack : BaseBuildingAttack
{
    private int _remainingAttackWaves = default;

    public SecondBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable, RepositoryManager repositoryManager)
        : base(buildingController, buildingScriptable, repositoryManager)
    {
        // Large amount of damage in a short period of time,- Waves
        _remainingAttackWaves = _buildingScriptable.AttackWave;
    }

    protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
    {
        IDamagable troopIDamagable = IDamagableTroopList[0];

        float attackRange = _buildingScriptable.AttackRange;

        float reloadingTime = _buildingScriptable.ReloadingTime;
        float timeBetweenWaves = _buildingScriptable.TimeBetweenWaves;

        yield return new WaitForSeconds(_reactionTime);

        for ( ; _remainingAttackWaves > 0; _remainingAttackWaves--)
        {
            if (isTroopStillAlive(troopIDamagable, out Transform troopTransform) == false)
                break;

            Vector3 buildingPosition = _buildingController.transform.position;
            Vector3 troopPosition = troopTransform.position;

            if (Vector3.Distance(buildingPosition, troopPosition) > attackRange)
                break;

            Attack(troopIDamagable);

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        yield return ReloadAttack();
    }

    private IEnumerator ReloadAttack()
    {
        int totalAttackWavesCount = _buildingScriptable.AttackWave;

        for ( ; _remainingAttackWaves < totalAttackWavesCount + 1; _remainingAttackWaves++)
        {
            float reloadingTime = _buildingScriptable.ReloadingTime;

            yield return new WaitForSeconds(reloadingTime / totalAttackWavesCount);
        }
    }

    protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy)
    {
        IDamagable IDamagableTroop = _repositoryManager.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false) as IDamagable;

        return new IDamagable[] { IDamagableTroop };
    }
}
