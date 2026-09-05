using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Bullet;
using App.Scripts.Core.ObjectPool;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.Defense_State;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Infrastructure.Others;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Core.Buildings.Base
{
    public abstract class BaseBuildingBehaviour
    {
        private readonly Dictionary<Transform, Quaternion> _initialLocalRotations = new Dictionary<Transform, Quaternion>();

        protected readonly BuildingController _buildingController;
        protected readonly BuildingScriptable _buildingScriptable;

        protected readonly TargetSearchService _targetSearchService;
        protected readonly ICoroutineRunner _coroutineRunner;

        protected readonly List<Transform> _bulletInitialPointList;
        protected readonly List<GameObject> _rotatingObjectList;

        protected readonly Transform _observePoint;

        protected const float _reactionTime = 0.5f;

        private const float _rotationSpeed = 120f;

        public bool IsAttacking { get; private set; }

        protected abstract int _maxRotateAngleFromCenter { get; }

        protected Faction _buildingSide = Faction.Enemies;

        public BaseBuildingBehaviour(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, List<GameObject> rotatingObjectList, Transform observePoint, ICoroutineRunner coroutineRunner)
        {
            _buildingController = buildingController;
            _buildingScriptable = buildingScriptable;

            _targetSearchService = targetSearchService;
            _coroutineRunner = coroutineRunner;

            _bulletInitialPointList = bulletInitialPointList;
            _rotatingObjectList = rotatingObjectList;

            _observePoint = observePoint;

            SetupRotatingObjectDictionary();
        }

        private void SetupRotatingObjectDictionary()
        {
            foreach (GameObject obj in _rotatingObjectList)
            {
                _initialLocalRotations[obj.transform] = obj.transform.localRotation;
            }
        }

        public void CheckAndTryToAttackEnemy(IDamagable targetPriorityEnemy = null)
        {
            Vector3 buildingCenter = _buildingController.transform.position;
            Faction targetFaction = _buildingSide.GetOpposite();

            float attackRange = _buildingScriptable.AttackRange;

            IDamagable[] enemiesDamagableArray = GetEnemyTargets(buildingCenter, attackRange, targetFaction, targetPriorityEnemy);

            if (enemiesDamagableArray != null && enemiesDamagableArray.Length > 0 && isTargetWithinAngle(enemiesDamagableArray))
            {
                _buildingController.StartCoroutine(ExecuteAttackCoroutine(enemiesDamagableArray));
            }
        }

        protected IEnumerator Attack(IDamagable attackTarget, Vector3 initialBulletPosition)
        {
            if (isTroopStillAlive(attackTarget, out Transform troopTransform) == false)
                yield break;

            Vector3 targetBulletPosition = new Vector3(troopTransform.position.x, initialBulletPosition.y, troopTransform.transform.position.z);

            yield return InitializeBullet(initialBulletPosition, targetBulletPosition);

            int damage = _buildingScriptable.Damage;
            attackTarget.TakeDamage(damage);

            IReactableForDamage enemyReactableForDamage = troopTransform.gameObject.GetComponent<IReactableForDamage>();
            enemyReactableForDamage?.ReactionForTakingDamage(_buildingController);
        }

        private IEnumerator InitializeBullet(Vector3 initialBulletPosition, Vector3 targetBulletPosition)
        {
            BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
            bulletController.InitializeBullet(initialBulletPosition, targetBulletPosition);

            yield return new WaitForSeconds(bulletController.GetBulletLifetime());
        }

        protected bool isTroopStillAlive(IDamagable troopIDamagable, out Transform troopTransform)
        {
            UnityEngine.Object troopObject = troopIDamagable as UnityEngine.Object;

            MonoBehaviour troopMonobehaviour = troopObject as MonoBehaviour;
            troopTransform = troopMonobehaviour != null ? troopMonobehaviour.transform : null;

            if (troopObject == null || troopTransform == null)
                return false;

            return true;
        }

        protected bool isTargetWithinAngle(IDamagable[] enemiesDamagableArray)
        {
            if (enemiesDamagableArray == null || enemiesDamagableArray.Length == 0)
                return false;

            Vector3 attackerPosition = _buildingController.transform.position;
            Vector3 baseForward = Vector3.ProjectOnPlane(_observePoint.forward, Vector3.up).normalized;

            if (baseForward == Vector3.zero)
                baseForward = Vector3.forward;

            float debugDuration = 2.0f;

            Debug.DrawRay(attackerPosition, baseForward * 5f, Color.green, debugDuration);

            Quaternion leftBoundary = Quaternion.Euler(0, -_maxRotateAngleFromCenter, 0);
            Quaternion rightBoundary = Quaternion.Euler(0, _maxRotateAngleFromCenter, 0);
            Debug.DrawRay(attackerPosition, (leftBoundary * baseForward) * 4f, Color.red, debugDuration);
            Debug.DrawRay(attackerPosition, (rightBoundary * baseForward) * 4f, Color.red, debugDuration);

            foreach (IDamagable damagable in enemiesDamagableArray)
            {
                if (damagable is MonoBehaviour enemyMono && enemyMono != null)
                {
                    Vector3 enemyPosition = enemyMono.transform.position;
                    Vector3 directionToEnemy = Vector3.ProjectOnPlane(enemyPosition - attackerPosition, Vector3.up).normalized;

                    if (directionToEnemy == Vector3.zero)
                        continue;

                    Debug.DrawRay(attackerPosition, directionToEnemy * 4f, Color.yellow, debugDuration);

                    float angleToEnemy = Vector3.Angle(baseForward, directionToEnemy);

                    if (angleToEnemy <= _maxRotateAngleFromCenter)
                    {
                        RotateObjectsTowards(enemyPosition);
                        return true;
                    }
                }
            }

            return false;
        }

        private void RotateObjectsTowards(Vector3 targetPosition)
        {
            Vector3 baseForward = Vector3.ProjectOnPlane(_observePoint.forward, Vector3.up).normalized;

            if (baseForward == Vector3.zero)
                baseForward = Vector3.forward;

            Vector3 directionToEnemy = Vector3.ProjectOnPlane(targetPosition - _buildingController.transform.position, Vector3.up).normalized;

            if (directionToEnemy == Vector3.zero)
                return;

            Quaternion deltaRotation = Quaternion.FromToRotation(baseForward, directionToEnemy);

            foreach (GameObject obj in _rotatingObjectList)
            {
                if (_initialLocalRotations.TryGetValue(obj.transform, out Quaternion initialLocalRot))
                {
                    Quaternion initialWorldRotation = _buildingController.transform.rotation * initialLocalRot;
                    Quaternion targetWorldRotation = deltaRotation * initialWorldRotation;

                    obj.transform.DOKill();
                    obj.transform.DORotate(targetWorldRotation.eulerAngles, _rotationSpeed).SetSpeedBased(true).SetEase(Ease.Linear);
                }
            }
        }

        private IEnumerator ExecuteAttackCoroutine(IDamagable[] enemiesDamagableArray)
        {
            IsAttacking = true;
            yield return AttackCoroutine(enemiesDamagableArray);
            IsAttacking = false;
        }

        protected virtual IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetFaction, IDamagable targetPriorityEnemy)
        {
            var IDamagableTroop = _targetSearchService.GetClosestEnemyInRange(currentPosition, attackRange, targetFaction, targetPriorityEnemy, false);
            return new IDamagable[] { IDamagableTroop as IDamagable };
        }

        protected abstract IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList);
    }
}

