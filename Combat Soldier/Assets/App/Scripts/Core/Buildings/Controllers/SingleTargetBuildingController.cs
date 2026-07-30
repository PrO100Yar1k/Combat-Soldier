using Assets.App.Scripts.Core.Buildings.Strategies;

public class SingleTargetBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new SingleTargetAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPointList, _rotatingObjectList, _observePoint, _coroutineRunner);
    }
}