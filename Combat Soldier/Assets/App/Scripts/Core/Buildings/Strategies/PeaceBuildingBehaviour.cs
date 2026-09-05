using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Buildings.Strategies
{
    public class PeaceBuildingBehaviour : BaseBuildingBehaviour
    {
        protected override int _maxRotateAngleFromCenter => 0;

        public PeaceBuildingBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, rotatingObjectList, observePoint, coroutineRunner)
        {
            // peace building strategy
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            yield return null;
        }

        protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetFaction, IDamagable targetPriorityEnemy)
        {
            return null;
        }
    }
}
