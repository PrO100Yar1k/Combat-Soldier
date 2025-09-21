using System.Collections;
using UnityEngine;

public class AttackBuildingController : BuildingController
{
    protected override void InitializeBuildingBehaviour()
    {
        _attackable = new DefaultBuildingAttack(this, _buildingScriptable);

        StartCoroutine(_attackable.CheckAttackTargetCoroutine());
    }
}