using UnityEngine;

public class PlayerDeathState : TroopDeathState
{
    public PlayerDeathState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}
