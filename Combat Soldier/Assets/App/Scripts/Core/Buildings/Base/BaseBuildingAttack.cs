using System.Collections.Generic;
using Assets.App.Scripts;
using System.Collections;
using UnityEngine;

public abstract class BaseBuildingAttack
{
    protected readonly BuildingController _buildingController;
    protected readonly BuildingScriptable _buildingScriptable;

    protected readonly TargetSearchService _targetSearchService;
    protected readonly ICoroutineRunner _coroutineRunner;

    protected readonly List<Transform> _bulletInitialPointList;

    protected const float _checkTargetDelay = 1f;
    protected const float _reactionTime = 0.5f;

    protected abstract int _maxRotateAngle { get; }

    public BaseBuildingAttack(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, List<Transform> bulletInitialPointList, ICoroutineRunner coroutineRunner)
    {
        _buildingController = buildingController;
        _buildingScriptable = buildingScriptable;

        _targetSearchService = targetSearchService;
        _coroutineRunner = coroutineRunner;

        _bulletInitialPointList = bulletInitialPointList;
    }

    public virtual IEnumerator CheckAttackTargetCoroutine() // to do
    {
        Faction targetTroopSide = Faction.Allies;
        IDamagable targetPriorityEnemy = null;

        float attackRange = _buildingScriptable.AttackRange;

        while (true)
        {
            Vector3 buildingCenter = _buildingController.transform.position;

            IDamagable[] enemiesDamagableArray = GetEnemyTargets(buildingCenter, attackRange, targetTroopSide, targetPriorityEnemy);

            if (enemiesDamagableArray != null && enemiesDamagableArray.Length > 0 && isTargetWithinAngle(enemiesDamagableArray))
            {
                yield return _buildingController.StartCoroutine(AttackCoroutine(enemiesDamagableArray));
            }

            yield return new WaitForSeconds(_checkTargetDelay);
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

        if (troopObject == null)
            return false;

        if (troopTransform == null)
            return false;

        return true;
    }

    protected bool isTargetWithinAngle(IDamagable[] enemiesDamagableArray)
    {
        if (enemiesDamagableArray == null || enemiesDamagableArray.Length == 0)
            return false;

        Vector3 attackerPosition = _buildingController.transform.position;
        Vector3 attackerForward = _buildingController.transform.forward;

        foreach (IDamagable damagable in enemiesDamagableArray)
        {
            if (damagable is MonoBehaviour enemyMono && enemyMono != null)
            {
                Vector3 directionToEnemy = enemyMono.transform.position - attackerPosition;

                directionToEnemy.y = 0f;

                Vector3 forwardFlat = new Vector3(attackerForward.x, 0f, attackerForward.z);

                float angleToEnemy = Vector3.Angle(forwardFlat, directionToEnemy);

                if (angleToEnemy <= _maxRotateAngle)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected abstract IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList);

    protected abstract IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy);
}

