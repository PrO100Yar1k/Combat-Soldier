using UnityEngine;

public class EnemyDeathState : TroopDeathState
{
    public EnemyDeathState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}
