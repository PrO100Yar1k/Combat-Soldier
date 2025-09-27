using UnityEngine;

public class PlayerTroopDefaultState : TroopDefaultState
{
    public PlayerTroopDefaultState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }

    public override void Start()
    {
        EnableStateIcon();

        // enable idle animation
    }

    public override void Stop()
    {
        // disable idle animation
    }

}
