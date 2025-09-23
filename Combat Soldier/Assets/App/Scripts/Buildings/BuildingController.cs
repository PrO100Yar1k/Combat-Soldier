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

    protected BaseBuildingAttack _buildingAttack = default;

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

    public Transform ObjectTransform => this.transform;

    #endregion

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
    public Transform ObjectTransform { get; }
    public void TakeDamage(int attackDamage);
}

public interface IAttackable
{
    public IEnumerator CheckAttackTargetCoroutine();

    public void Attack(IDamagable attackTarget); //
}