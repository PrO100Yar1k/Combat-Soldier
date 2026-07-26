using Assets.App.Scripts.Core.Canvases;
using Assets.App.Scripts.Core.Health;
using UnityEngine;

public class HPBuildingController : HPController<BuildingScriptable>
{
    protected readonly BuildingScreenCanvasController _buildingCanvasController = default;

    private readonly BuildingController _buildingController = default;

    public HPBuildingController(BuildingController buildingController, BuildingScreenCanvasController buildingCanvasController, BuildingScriptable buildingScriptable) : base(buildingScriptable)
    {
        _buildingController = buildingController;
        _buildingCanvasController = buildingCanvasController;

        UpdateSliderAndTextValues();
    }

    protected override void InitializeData(BuildingScriptable buildingScriptable)
    {
        _unitName = buildingScriptable.Name;
        _currentHealPoint = buildingScriptable.MaxHealPoint;
    }

    protected override void UpdateSliderAndTextValues()
    {
        _buildingCanvasController.UpdateHealth(_currentHealPoint);
    }

    public override void TakeDamage(int attackDamage)
    {
        _currentHealPoint -= attackDamage;

        UpdateSliderAndTextValues();
        CheckHealPointsForDeath();
    }

    protected override void HandleDeath()
    {
        _buildingController.Dispose();
        _buildingController.StopAllCoroutines();

        UnityEngine.Object.Destroy(_buildingController.gameObject);
        Debug.Log($"The {_unitName} was died");
    }
}
