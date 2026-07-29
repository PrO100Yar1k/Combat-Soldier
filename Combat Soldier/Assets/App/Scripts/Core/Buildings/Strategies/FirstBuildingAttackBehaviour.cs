using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.App.Scripts.Core.Buildings.Strategies
{
    public class FirstBuildingAttackBehaviour : BaseBuildingAttack
    {
        protected override int _maxRotateAngleFromCenter => 45;

        public FirstBuildingAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, rotatingObjectList, observePoint, coroutineRunner)
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

                Vector3 troopPosition = troopTransform.position;
                Vector3 buildingPosition = _buildingController.transform.position;

                if (Vector3.Distance(buildingPosition, troopPosition) > attackRange || !isTargetWithinAngle(new[] { troopIDamagable }))
                    yield break;

                Vector3 initialBulletPoint = _bulletInitialPointList[0].position;
                _coroutineRunner.StartCoroutine(Attack(troopIDamagable, initialBulletPoint));

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