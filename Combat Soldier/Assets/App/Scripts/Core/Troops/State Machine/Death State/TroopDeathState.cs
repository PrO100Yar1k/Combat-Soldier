
public abstract class TroopDeathState : TroopBaseState
{
    public TroopDeathState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        // maybe remove this class ?
    }

    public override void Stop()
    {

    }

    protected override void EnableStateIcon()
    {

    }
}
