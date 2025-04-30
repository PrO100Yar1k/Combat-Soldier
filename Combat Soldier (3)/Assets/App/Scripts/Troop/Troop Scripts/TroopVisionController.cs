using System.Collections;
using UnityEngine;

public class TroopVisionController
{
    public bool IsTroopVisible { get; private set; } = true;

    private TroopController _troopController = default;
    private TroopScriptable _troopScriptable = default;

    public TroopVisionController(TroopController troopController, TroopScriptable troopScriptable, TroopSide troopSide) // to do
    {
        _troopController = troopController;
        _troopScriptable = troopScriptable;
    }

    public TroopController[] GetEnemiesInVisionRange()
    {
        Vector3 currentPosition = _troopController.transform.position;
        float viewRange = _troopScriptable.ViewRangeRadius;

        TroopSide enemyTroopSide = GetEnemyTroopSide();

        return TroopGeneralManager.instance.GetEnemyListInRange(currentPosition, viewRange, enemyTroopSide);
    }

    private TroopSide GetEnemyTroopSide()
        => _troopScriptable.TroopSide == TroopSide.Player ? TroopSide.Enemy : TroopSide.Player;
}
