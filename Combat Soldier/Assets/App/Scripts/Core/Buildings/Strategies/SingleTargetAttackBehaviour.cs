using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Buildings.Strategies
{
    public class SingleTargetAttackBehaviour : BaseBuildingBehaviour
    {
        protected override int _maxRotateAngleFromCenter => 65;

        public SingleTargetAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, rotatingObjectList, observePoint, coroutineRunner)
        {
            // Default attack without waves, just usual attack with fixed reloading time
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            IDamagable troopIDamagable = IDamagableTroopList[0];

            const float fixedTimeDelay = 0.25f;
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

                yield return new WaitForSeconds(fixedTimeDelay);

                Vector3 initialBulletPoint = _bulletInitialPointList[0].position;
                _coroutineRunner.StartCoroutine(Attack(troopIDamagable, initialBulletPoint));

                yield return new WaitForSeconds(reloadingTime);
            }
        }
    }
}