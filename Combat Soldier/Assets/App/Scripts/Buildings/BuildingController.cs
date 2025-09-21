using System.Collections;
using UnityEngine;

public abstract class BuildingController : MonoBehaviour, IDamagable, System.IDisposable
{
    [SerializeField] protected BuildingScriptable _buildingScriptable = default;

    [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController = default;
    [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController = default;

    public UICanvasController<BuildingController> UIController { get; protected set; }
    public HPControllerBuilding HPController { get; protected set; }

    public BuildingScriptable BuildingScriptable => _buildingScriptable;

    protected IAttackable _attackable = default;

    #region Events & Interface Implemention

    protected virtual void OnEnable()
        => GameEvents.instance.BuildingSpawned(this);

    protected virtual void OnDisable()
        => GameEvents.instance.BuildingDestroyed(this);

    private void Awake()
        => InitializeBuilding();

    public void Dispose()
        => UIController.Dispose();

    public void TakeDamage(int attackDamage)
        => HPController.TakeDamage(attackDamage);

    #endregion

    public void Attack(IDamagable attackTarget)
        => _attackable?.Attack(attackTarget);

    public IEnumerator AttackPlayerCoroutine(IDamagable playerTroopController)
    {
        float attackRange = _buildingScriptable.AttackRange;

        while (playerTroopController != null && Vector3.Distance(transform.position, playerTroopController.transform.position) < attackRange)
        {
            const float reactionTime = 0.5f;
            yield return new WaitForSeconds(reactionTime);

            _attackable?.Attack(playerTroopController);

            float timeToWait = _buildingScriptable.TimeToReload;
            yield return new WaitForSeconds(timeToWait);
        }
    }

    protected virtual void InitializeBuilding()
    {
        UIController = new UICanvasController<BuildingController>(this, _buildingScreenCanvasController, _buildingWorldCanvasController);
        HPController = new HPControllerBuilding(this, _buildingScreenCanvasController, _buildingScriptable);

        InitializeBuildingBehaviour();
    }

    protected abstract void InitializeBuildingBehaviour();
}


public interface IDamagable
{
    public Transform transform { get; }
    public void TakeDamage(int attackDamage);
}

public interface IAttackable
{
    public IEnumerator CheckAttackTargetCoroutine();

    public void Attack(IDamagable attackTarget);
}