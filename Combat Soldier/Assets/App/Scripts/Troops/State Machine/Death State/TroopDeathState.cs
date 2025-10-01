
public abstract class TroopDeathState : TroopBaseState
{
    public TroopDeathState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

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
