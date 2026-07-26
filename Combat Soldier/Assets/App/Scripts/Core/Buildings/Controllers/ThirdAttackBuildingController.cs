using Assets.App.Scripts.Core.Buildings.Strategies;

public class ThirdAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new ThirdBuildingAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPointList, _coroutineRunner);
        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
