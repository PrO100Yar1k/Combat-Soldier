using Assets.App.Scripts.Core.Buildings.Strategies;

public class PeaceBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new NoBuildingAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPoint);
    }
}
