using Assets.App.Scripts;

public class PlayerDeathState : TroopDeathState
{
    public PlayerDeathState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }
}
