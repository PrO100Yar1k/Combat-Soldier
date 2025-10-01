using UnityEngine;

public class PlayerMoveState : TroopMoveState
{
    public PlayerMoveState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}
