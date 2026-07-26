using UnityEngine;
using System.Collections;

namespace Assets.App.Scripts.Core.Buildings.Strategies
{
    public class FirstBuildingAttackBehaviour : BaseBuildingAttack
    {
        public FirstBuildingAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, Transform bulletInitialPoint)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPoint)
        {
            // Default attack without waves, just usual attack with fixed reloading time
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            IDamagable troopIDamagable = IDamagableTroopList[0];

            float attackRange = _buildingScriptable.AttackRange;
            float reloadingTime = _buildingScriptable.ReloadingTime;

            yield return new WaitForSeconds(_reactionTime);

            while (true)
            {
                if (isTroopStillAlive(troopIDamagable, out Transform troopTransform) == false)
                    yield break;

                Vector3 buildingPosition = _bulletInitialPoint.position;
                Vector3 troopPosition = troopTransform.position;

                if (Vector3.Distance(buildingPosition, troopPosition) > attackRange)
                    yield break;

                Attack(troopIDamagable);

                yield return new WaitForSeconds(reloadingTime);
            }
        }

        protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy)
        {
            IDamagable IDamagableTroop = _targetSearchService.GetClosestEnemyInRange(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy, false) as IDamagable;

            return new IDamagable[] { IDamagableTroop };
        }
    }
}