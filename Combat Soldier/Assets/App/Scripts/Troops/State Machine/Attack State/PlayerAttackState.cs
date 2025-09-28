using UnityEngine;

public class PlayerAttackState : TroopAttackState
{
    public PlayerAttackState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {
        _enemyTroopSide = TroopSide.Enemy;
    }
}
