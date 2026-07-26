using Assets.App.Scripts.Core.Buildings.Strategies;

public class FirstAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new FirstBuildingAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPointList, _coroutineRunner);
        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}