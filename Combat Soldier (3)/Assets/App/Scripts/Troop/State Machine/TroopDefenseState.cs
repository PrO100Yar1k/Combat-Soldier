using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    public TroopDefenseState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState) { }

    public override void Start()
    {
        Debug.Log("Entered to Defense State!");
    }

    public override void Stop()
    {
        Debug.Log("Exit from Defense State!");
    }
}
