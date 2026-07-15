using Assets.App.Scripts;

public abstract class TroopDeathState : TroopBaseState // maybe remove this class
{
    protected override string StateIconLocation
        => "State Icons/Death-State-Icon";

    protected TroopDeathState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    public override void OnStart()
    {
        PlayStateAnimation();
    }

    public override void OnStop()
    {

    }

    protected override void PlayStateAnimation()
    {
        //_animatorController.PlayDeath();
    }
}
