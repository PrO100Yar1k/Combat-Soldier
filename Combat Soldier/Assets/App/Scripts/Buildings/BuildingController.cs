using UnityEngine;

public abstract class BuildingController : MonoBehaviour, IDamagable, System.IDisposable
{
    [SerializeField] protected BuildingScriptable _buildingScriptable = default;

    [Space(2)]

    [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController = default;
    [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController = default;

    public UICanvasController<BuildingController> UIController { get; protected set; }
    public HPControllerBuilding HPController { get; protected set; }

    public BuildingScriptable BuildingScriptable => _buildingScriptable;

    [SerializeField] protected TroopController _troopInBuilding = default; //

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

    protected abstract void InitializeBuilding();

    public void InitializeTroopInsideBuilding(TroopController troopController)
    {
        if (troopController == null)
            return;

        _troopInBuilding = troopController;
    }

    public TroopController GetTroopInsideBuilding()
        => _troopInBuilding;
}

public interface IDamagable
{
    public void TakeDamage(int attackDamage);
}