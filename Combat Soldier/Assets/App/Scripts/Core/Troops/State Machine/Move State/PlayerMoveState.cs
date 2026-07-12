using UnityEngine;

public class PlayerMoveState : TroopMoveState
{
    public PlayerMoveState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }
}
