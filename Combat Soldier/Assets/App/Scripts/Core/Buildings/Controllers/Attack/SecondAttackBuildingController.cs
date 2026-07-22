
public class SecondAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new SecondBuildingAttack(this, _buildingScriptable, _targetSearchService);
        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
