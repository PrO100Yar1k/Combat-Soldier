using UnityEngine;

public class EnemyMoveState : TroopMoveState
{
    public EnemyMoveState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }
}
