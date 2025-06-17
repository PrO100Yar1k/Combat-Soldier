using UnityEngine;

public class BuildingController : MonoBehaviour, IDamagable // make this class abstract
{
    [SerializeField] private BuildingScriptable _buildingScriptable = default;

    [Space(2)]

    [SerializeField] protected ScreenCanvasController _screenCanvasController = default;

    //[SerializeField] protected WorldCanvasController _worldCanvasController = default;

    private HPControllerBuilding HPController; // { get; private set; }

    private TroopController _troopInsideBuilding = default; // [SerializeField] 

    private void Awake()
    {
        InitializeBuilding();
    }

    private void InitializeBuilding()
    {
        HPController = new HPControllerBuilding(this, _screenCanvasController, _buildingScriptable);
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

public interface IDamagable     // ???
{
    public void TakeDamage(int attackDamage);
}