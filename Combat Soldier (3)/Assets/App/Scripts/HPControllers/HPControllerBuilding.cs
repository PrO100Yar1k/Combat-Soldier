
public class HPControllerBuilding : HPController
{
    private readonly BuildingController _buildingController = default;

    public HPControllerBuilding(BuildingController buildingController, ScreenCanvasController troopCanvasController, BuildingScriptable buildingScriptable) : base(troopCanvasController)
    {
        _buildingController = buildingController;

        AssignBasicParameters(buildingScriptable);
        ChangeSliderAndTextValues();
    }

    private void AssignBasicParameters(BuildingScriptable buildingScriptable)
    {
        _currentName = buildingScriptable.Name;
        _currentHealPoint = buildingScriptable.MaxHealPoint;
    }

    protected override void ChangeSliderAndTextValues()
    {
        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
    }

    public override void TakeDamage(int attackDamage)
    {
        _currentHealPoint -= attackDamage;

        ChangeSliderAndTextValues();

        CheckHealPointsForTroopDeath();
    }

    protected override void CheckHealPointsForTroopDeath()
    {
        if (_currentHealPoint <= 0)
            base.TroopDeath(_buildingController, _buildingController.gameObject);
    }
}
