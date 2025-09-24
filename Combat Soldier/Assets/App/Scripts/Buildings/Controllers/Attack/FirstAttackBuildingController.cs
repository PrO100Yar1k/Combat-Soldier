
public class FirstAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new FirstBuildingAttack(this, _buildingScriptable);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}