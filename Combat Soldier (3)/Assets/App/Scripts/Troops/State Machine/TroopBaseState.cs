
public abstract class TroopBaseState
{
    protected ScreenCanvasController _screenCanvasController = default;

    protected TroopController _troopController = default;

    protected ISwitchableState _switcherState = default;

    protected TroopScriptable _troopScriptable = default;

    public TroopBaseState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState)
    {
        _troopController = troopController;
        _switcherState = switcherState;

        _screenCanvasController = screenCanvasController;
        _troopScriptable = troopController.TroopScriptable;
    }

    public abstract void Start();

    public abstract void Stop();

    protected abstract void EnableStateIcon();
}
