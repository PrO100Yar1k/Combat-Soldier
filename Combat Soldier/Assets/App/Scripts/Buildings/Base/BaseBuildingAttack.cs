using System.Collections;
using UnityEngine;
using Zenject;

public abstract class BaseBuildingAttack
{
    protected readonly BuildingController _buildingController = default;
    protected readonly BuildingScriptable _buildingScriptable = default;

    protected const float _checkTargetDelay = 1f;
    protected const float _reactionTime = 0.5f;

    protected RepositoryManager _repositoryManager = default;

    public BaseBuildingAttack(BuildingController buildingController, BuildingScriptable buildingScriptable)
    {
        _buildingController = buildingController;
        _buildingScriptable = buildingScriptable;
    }

    [Inject]
    public void Construct(RepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public virtual IEnumerator CheckAttackTargetCoroutine()
    {
        Vector3 currentPosition = _buildingController.transform.position;
        float attackRange = _buildingScriptable.AttackRange;

        TroopSide targetTroopSide = TroopSide.Player;
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

    protected void Attack(IDamagable attackTarget)
    {
        if (isTroopStillAlive(attackTarget, out Transform troopTransform) == false)
            return;

        IReactableForDamage enemyReactableForDamage = troopTransform as IReactableForDamage;

        int damage = _buildingScriptable.Damage;

        attackTarget.TakeDamage(damage);
        enemyReactableForDamage?.ReactionForTakingDamage(_buildingController); // ?
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

    protected abstract IDamagable[] GetEnemyTargets(Vector3 currentPosition, float attackRange, TroopSide targetTroopSide, IDamagable targetPriorityEnemy);
}

