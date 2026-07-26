using System.Collections;
using UnityEngine;

namespace Assets.App.Scripts.Core.Buildings.Strategies
{
    public class NoBuildingAttackBehaviour : BaseBuildingAttack
    {
        public NoBuildingAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, Transform bulletInitialPoint)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPoint)
        {
            // peace building strategy
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            yield return null;
        }

        protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy)
        {
            return null;
        }
    }
}
