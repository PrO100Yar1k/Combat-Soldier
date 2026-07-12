
public class FirstAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new FirstBuildingAttack(this, _buildingScriptable, _repositoryManager);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}