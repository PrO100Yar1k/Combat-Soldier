using UnityEngine;

public class BuildingController : MonoBehaviour, IDamagable // make this class abstract
{
    [SerializeField] private BuildingScriptable _buildingScriptable = default;

    [Space(2)]

    [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController = default;
    [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController = default;

    //[SerializeField] protected WorldCanvasController _worldCanvasController = default;

    public UICanvasController<BuildingController> UIController { get; private set; }
    public HPControllerBuilding HPController { get; private set; }

    public BuildingScriptable BuildingScriptable => _buildingScriptable;

    private TroopController _troopInsideBuilding = default; // [SerializeField] 

    private void Awake()
        => InitializeBuilding();

    private void InitializeBuilding()
    {
        UIController = new UICanvasController<BuildingController>(this, _buildingScreenCanvasController, _buildingWorldCanvasController);
        HPController = new HPControllerBuilding(this, _buildingScreenCanvasController, _buildingScriptable);
    }

    public void TakeDamage(int attackDamage)
    {
        HPController.TakeDamage(attackDamage);
    }

    public void GetTroopInsideBuilding(TroopController troopController)
    {
        if (_troopInsideBuilding != null)
            return;

        _troopInsideBuilding = troopController;
    }
}

public interface IDamagable
{
    public void TakeDamage(int attackDamage);
}