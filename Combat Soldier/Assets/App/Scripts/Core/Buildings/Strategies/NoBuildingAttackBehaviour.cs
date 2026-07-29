using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.App.Scripts.Core.Buildings.Strategies
{
    public class NoBuildingAttackBehaviour : BaseBuildingAttack
    {
        public NoBuildingAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, rotatingObjectList, observePoint, coroutineRunner)
        {
            // peace building strategy
        }

        protected override int _maxRotateAngleFromCenter => 0;

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
