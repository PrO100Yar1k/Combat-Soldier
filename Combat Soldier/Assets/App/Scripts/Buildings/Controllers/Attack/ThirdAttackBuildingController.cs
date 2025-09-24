using UnityEngine;

public class ThirdAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new ThirdBuildingAttack(this, _buildingScriptable);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
