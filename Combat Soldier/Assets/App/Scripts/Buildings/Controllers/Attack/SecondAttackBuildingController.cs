
public class SecondAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new SecondBuildingAttack(this, _buildingScriptable, _repositoryManager);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
