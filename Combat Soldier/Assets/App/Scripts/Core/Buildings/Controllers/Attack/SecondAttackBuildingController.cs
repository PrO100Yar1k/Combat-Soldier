using Assets.App.Scripts.Core.Buildings.Strategies;

public class SecondAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new SecondBuildingAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPoint);
        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
