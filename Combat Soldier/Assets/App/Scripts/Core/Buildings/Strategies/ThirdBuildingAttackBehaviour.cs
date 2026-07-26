using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.App.Scripts.Core.Buildings.Strategies
{ 
    public class ThirdBuildingAttackBehaviour : BaseBuildingAttack
    {
        private readonly Queue<Transform> _bulletPointQueue = new Queue<Transform>();

        private const int _attackCannonCount = 2;

        public ThirdBuildingAttackBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, ICoroutineRunner coroutineRunner)
            : base(buildingController, targetSearchService, buildingScriptable, bulletInitialPointList, coroutineRunner)
        {
            foreach (Transform targetPoint in bulletInitialPointList)
                _bulletPointQueue.Enqueue(targetPoint);

            // damage all enemies (above _maxAttackUnitCount, but could make infinite enemy count too) in the attack range based on usual reload  // to do (edit this strategy)
        }

        protected override IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList)
        {
            float attackRange = _buildingScriptable.AttackRange;
            float reloadingTime = _buildingScriptable.ReloadingTime;

            Faction targetTroopSide = Faction.Allies;
            IDamagable targetPriorityEnemy = null;

            yield return new WaitForSeconds(_reactionTime);

            Vector3 buildingCenter = _buildingController.transform.position;

            while (true)
            {
                IDamagableTroopList = GetEnemyTargets(buildingCenter, attackRange, targetTroopSide, targetPriorityEnemy);

                if (IDamagableTroopList == null || IDamagableTroopList.Length == 0)
                    yield break;

                IDamagable troopIDamagable = IDamagableTroopList[0];

                if (isTroopStillAlive(troopIDamagable, out Transform troopTransform) == false)
                    yield break;

                Vector3 troopPosition = troopTransform.position;

                if (Vector3.Distance(buildingCenter, troopPosition) > attackRange)
                    yield break;

                float timeBetweenWaves = _buildingScriptable.TimeBetweenWaves;

                for (int j = 0; j < _attackCannonCount; j++)
                {
                    Vector3 initialBulletPoint = GetNextBulletPointTransform().position;
                    _coroutineRunner.StartCoroutine(Attack(troopIDamagable, initialBulletPoint));

                    yield return new WaitForSeconds(timeBetweenWaves);
                }

                yield return new WaitForSeconds(reloadingTime);
            }
        }

        private Transform GetNextBulletPointTransform()
        {
            if (_bulletPointQueue.Count == 0)
                return _buildingController.transform;

            Transform targetPoint = _bulletPointQueue.Dequeue();
            _bulletPointQueue.Enqueue(targetPoint);

            return targetPoint;
        }

        protected override IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy)
        {
            return _targetSearchService.GetEnemyListInRange(currentPosition, attackRange, targetTroopSide)
                ?.Take(_attackCannonCount).ToArray();
        }
    }
}
