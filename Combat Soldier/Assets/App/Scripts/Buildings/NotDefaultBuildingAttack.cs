using System.Collections;
using UnityEngine;

public class NotDefaultBuildingAttack : BaseBuildingAttack
{
    private int _remainingAttackWaves = default;

    public NotDefaultBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable) : base(buildingController, buildingScriptable)
    {
        _remainingAttackWaves = _buildingScriptable.AttackWave;
    }

    protected override IEnumerator AttackCoroutine(IDamagable troopIDamagable)
    {
        float attackRange = _buildingScriptable.AttackRange;

        float reloadingTime = _buildingScriptable.ReloadingTime;
        float timeBetweenWaves = _buildingScriptable.TimeBetweenWaves;

        yield return new WaitForSeconds(_reactionTime);

        for ( ; _remainingAttackWaves > 0; _remainingAttackWaves--)
        {
            Transform troopTransform = default;

            if (isTroopStillAlive(troopIDamagable, out troopTransform) == false)
                yield break;

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

    protected override IDamagable GetTargetEnemy(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy)
        => TroopGeneralManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy) as IDamagable;
}
