using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Buildings.Strategies
{ 
    public class WaveAttackBehaviour : BaseBuildingBehaviour
    {
        private int _remainingAttackWaves = default;

        protected override int _maxRotateAngleFromCenter => 45;

        public WaveAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, rotatingObjectList, observePoint, coroutineRunner)
        {
            _remainingAttackWaves = _buildingScriptable.AttackWave;

            // Large amount of damage in a short period of time,- Waves Attack Type
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            IDamagable troopIDamagable = IDamagableTroopList[0];

            const float fixedTimeDelay = 0.3f;
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

                if (Vector3.Distance(buildingPosition, troopPosition) > attackRange || !isTargetWithinAngle(new[] { troopIDamagable }))
                    break;

                yield return new WaitForSeconds(fixedTimeDelay);

                Vector3 initialBulletPoint = _bulletInitialPointList[0].position;
                _coroutineRunner.StartCoroutine(Attack(troopIDamagable, initialBulletPoint));

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
    }
}
