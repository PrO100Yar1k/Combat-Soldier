using UnityEngine;

public class EnemyDeathState : TroopDeathState
{
    public EnemyDeathState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }
}
