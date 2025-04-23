
public abstract class TroopBaseState
{
    protected TroopController _troopController = default;

    protected ISwitchableState _switcherState = default;

    public TroopBaseState(TroopController troopController, ISwitchableState switcherState)
    {
        _troopController = troopController;
        _switcherState = switcherState;
    }

    public abstract void Start();

    public abstract void Stop();
}
