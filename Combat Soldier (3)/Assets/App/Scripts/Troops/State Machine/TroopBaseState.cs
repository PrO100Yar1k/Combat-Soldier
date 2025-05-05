
public abstract class TroopBaseState
{
    protected TroopController _troopController = default;

    protected ISwitchableState _switcherState = default;

    protected TroopScriptable _troopScriptable = default;

    public TroopBaseState(TroopController troopController, ISwitchableState switcherState)
    {
        _troopController = troopController;
        _switcherState = switcherState;

        _troopScriptable = troopController.TroopScriptable;
    }

    public abstract void Start();

    public abstract void Stop();
}
