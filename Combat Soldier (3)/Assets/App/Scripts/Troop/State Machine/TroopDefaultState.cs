using UnityEngine;

public class TroopDefaultState : TroopBaseState
{
    public TroopDefaultState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState) { }

    public override void Start()
    {
        Debug.Log("Default State Entered!");
    }

    public override void Stop()
    {

    }
}
