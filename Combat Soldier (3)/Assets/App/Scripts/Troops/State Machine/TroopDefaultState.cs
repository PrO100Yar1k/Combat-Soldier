using UnityEngine;

public class TroopDefaultState : TroopBaseState
{
    public TroopDefaultState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState) { }

    public override void Start()
    {
        // enable idle animation
    }

    public override void Stop()
    {
        // disable idle animation
    }
}
