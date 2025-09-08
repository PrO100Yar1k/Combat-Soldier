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

    public void Attack(TroopController attackTarget)
        => _attackable?.Attack(attackTarget);

    protected void InitializeBuilding()
    {
        UIController = new UICanvasController<BuildingController>(this, _buildingScreenCanvasController, _buildingWorldCanvasController);
        HPController = new HPControllerBuilding(this, _buildingScreenCanvasController, _buildingScriptable);

        InitializeBuildingBehaviour();
    }

    protected abstract void InitializeBuildingBehaviour();
}


public interface IDamagable
{
    public void TakeDamage(int attackDamage);
}

public interface IAttackable
{
    public void Attack(IDamagable attackTarget);
}