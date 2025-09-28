using UnityEngine;

public class EnemyAttackState : TroopAttackState
{
    public EnemyAttackState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {
        _enemyTroopSide = TroopSide.Player;
    }
}
