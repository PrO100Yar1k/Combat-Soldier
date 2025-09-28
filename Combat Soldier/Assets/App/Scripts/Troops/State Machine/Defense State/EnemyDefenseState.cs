using UnityEngine;

public class EnemyDefenseState : TroopDefenseState
{
    public EnemyDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}

