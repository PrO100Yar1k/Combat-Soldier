using UnityEngine;

public class SecondAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new SecondBuildingAttack(this, _buildingScriptable);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}
