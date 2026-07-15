using Assets.App.Scripts;

public class EnemyDefenseState : TroopDefenseState
{
    public EnemyDefenseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}

