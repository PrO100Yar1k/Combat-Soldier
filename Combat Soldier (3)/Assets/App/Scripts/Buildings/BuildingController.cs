using UnityEngine;

public class BuildingController : MonoBehaviour, IDamagable
{
    [SerializeField] protected ScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected WorldCanvasController _worldCanvasController = default;

    [SerializeField] private BuildingScriptable _buildingScriptable = default;

    [SerializeField] private TroopController _troopInsideBuilding = default; // maybe make just private

    public HPControllerBuilding HPController { get; private set; }

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