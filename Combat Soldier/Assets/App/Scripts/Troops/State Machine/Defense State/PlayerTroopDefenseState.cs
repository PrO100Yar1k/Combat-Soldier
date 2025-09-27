using UnityEngine;

public class PlayerTroopDefenseState : TroopDefenseState
{
    public PlayerTroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {

    }
}
