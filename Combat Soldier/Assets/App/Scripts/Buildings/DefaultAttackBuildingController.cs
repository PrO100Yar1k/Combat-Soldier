using System.Collections;
using UnityEngine;

public class DefaultAttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _buildingAttack = new DefaultBuildingAttack(this, _buildingScriptable);

        StartCoroutine(_buildingAttack.CheckAttackTargetCoroutine());
    }
}