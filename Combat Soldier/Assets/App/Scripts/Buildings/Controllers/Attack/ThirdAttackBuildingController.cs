
public class ThirdAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new ThirdBuildingAttack(this, _buildingScriptable, _repositoryManager);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
