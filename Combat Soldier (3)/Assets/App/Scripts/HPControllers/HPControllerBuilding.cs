
public class HPControllerBuilding : HPController
{
    protected readonly BuildingScreenCanvasController _buildingCanvasController = default;

    private readonly BuildingController _buildingController = default;

    public HPControllerBuilding(BuildingController buildingController, BuildingScreenCanvasController buildingCanvasController, BuildingScriptable buildingScriptable)
    {
        _buildingController = buildingController;
        _buildingCanvasController = buildingCanvasController;

        AssignBasicParameters(buildingScriptable);
        ChangeSliderAndTextValues();
    }

    protected override void AssignBasicParameters<T>(T scriptableObject)
    {
        BuildingScriptable buildingScriptable = scriptableObject as BuildingScriptable;

        HPControllerName = buildingScriptable.Name;
        _currentHealPoint = buildingScriptable.MaxHealPoint;
    }

    protected override void ChangeSliderAndTextValues()
    {
        _buildingCanvasController.ChangeHealPointSlider(_currentHealPoint);
    }

    public override void TakeDamage(int attackDamage)
    {
        _currentHealPoint -= attackDamage;

        ChangeSliderAndTextValues();
        CheckHealPointsForDeath();
    }

    protected override void CheckHealPointsForDeath()
    {
        if (_currentHealPoint <= 0)
            base.TroopDeath(_buildingController, _buildingController.gameObject);
    }
}
