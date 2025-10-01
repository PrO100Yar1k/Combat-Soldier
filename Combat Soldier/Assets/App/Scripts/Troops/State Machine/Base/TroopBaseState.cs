using System;

public abstract class TroopBaseState : IDisposable
{
    protected TroopScreenCanvasController _screenCanvasController = default;

    protected TroopController _troopController = default;

    protected ISwitchableState _switcherState = default;

    protected TroopScriptable _troopScriptable = default;

    public TroopBaseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
    {
        _troopController = troopController;
        _switcherState = switcherState;

        _screenCanvasController = screenCanvasController;
        _troopScriptable = troopController.TroopScriptable;
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    public abstract void Start();

    public abstract void Stop();

    protected abstract void EnableStateIcon();

    protected virtual void SubscribeToEvents() { }

    protected virtual void UnSubscribeFromEvents() { }
}
