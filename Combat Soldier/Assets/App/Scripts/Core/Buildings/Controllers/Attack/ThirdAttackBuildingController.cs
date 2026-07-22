
public class ThirdAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new ThirdBuildingAttack(this, _buildingScriptable, _targetSearchService);
        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
