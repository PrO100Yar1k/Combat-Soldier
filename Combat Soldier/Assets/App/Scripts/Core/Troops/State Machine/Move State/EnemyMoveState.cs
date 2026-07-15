using Assets.App.Scripts;

public class EnemyMoveState : TroopMoveState
{
    public EnemyMoveState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}
