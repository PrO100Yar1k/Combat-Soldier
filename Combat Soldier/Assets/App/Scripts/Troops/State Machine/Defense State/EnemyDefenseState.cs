using UnityEngine;

public class EnemyDefenseState : TroopDefenseState
{
    public EnemyDefenseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState)
    {

    }
}

