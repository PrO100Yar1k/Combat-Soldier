using UnityEngine;

public class PlayerDeathState : TroopDeathState
{
    public PlayerDeathState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }
}
