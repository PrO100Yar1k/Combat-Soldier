using UnityEngine;

public class TroopDeathState : TroopBaseState
{
    public TroopDeathState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        // enable death animation
    }

    public override void Stop()
    {

    }

    protected override void EnableStateIcon()
    {
        // to do ?
    }
}
