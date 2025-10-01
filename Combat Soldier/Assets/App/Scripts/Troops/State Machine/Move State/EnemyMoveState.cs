using UnityEngine;

public class EnemyMoveState : TroopMoveState
{
    public EnemyMoveState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}
