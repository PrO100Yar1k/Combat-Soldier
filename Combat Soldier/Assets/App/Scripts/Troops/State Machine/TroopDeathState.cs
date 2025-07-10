using UnityEngine;

public class TroopDeathState : TroopBaseState
{
    public TroopDeathState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        // enable death animation
    }

    public override void Stop()
    {
        // maybe remove this class ???
    }

    protected override void EnableStateIcon()
    {
        // to do ?
    }
}
