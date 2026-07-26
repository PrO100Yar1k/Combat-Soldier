using System.Collections;
using UnityEngine;

public abstract class BaseBuildingAttack
{
    protected readonly BuildingController _buildingController;
    protected readonly BuildingScriptable _buildingScriptable;

    protected readonly TargetSearchService _targetSearchService;
    protected readonly Transform _bulletInitialPoint;

    protected const float _checkTargetDelay = 1f;
    protected const float _reactionTime = 0.5f;

    public BaseBuildingAttack(BuildingController buildingController, TargetSearchService targetSearchService, BuildingScriptable buildingScriptable, Transform bulletInitialPoint)
    {
        _buildingController = buildingController;
        _buildingScriptable = buildingScriptable;

        _targetSearchService = targetSearchService;
        _bulletInitialPoint = bulletInitialPoint;
    }

    public virtual IEnumerator CheckAttackTargetCoroutine() // to do
    {
        Vector3 currentPosition = _bulletInitialPoint.position;
        float attackRange = _buildingScriptable.AttackRange;

        Faction targetTroopSide = Faction.Allies;
        IDamagable targetPriorityEnemy = null;

        while (true)
        {
            IDamagable[] playerTroopController = GetEnemyTargets(currentPosition, attackRange, targetTroopSide, targetPriorityEnemy);

            if (playerTroopController != null && playerTroopController.Length > 0)
            {
                yield return _buildingController.StartCoroutine(AttackCoroutine(playerTroopController));
            }

            yield return new WaitForSeconds(_checkTargetDelay);
        }
    }

    protected virtual void Attack(IDamagable attackTarget)
    {
        if (isTroopStillAlive(attackTarget, out Transform troopTransform) == false)
            return;

        IReactableForDamage enemyReactableForDamage = troopTransform as IReactableForDamage;

        int damage = _buildingScriptable.Damage;

        attackTarget.TakeDamage(damage);
        enemyReactableForDamage?.ReactionForTakingDamage(_buildingController);
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

    protected abstract IEnumerator AttackCoroutine(IDamagable[] IDamagableTroopList);

    protected abstract IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, Faction targetTroopSide, IDamagable targetPriorityEnemy);
}

