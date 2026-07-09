using System;
using Zenject;

public abstract class TroopBaseState : IDisposable
{
    protected readonly RepositoryManager _repositoryManager;

    protected readonly TroopController _troopController = default;
    protected readonly TroopScriptable _troopScriptable = default;
    protected readonly ISwitchableState _switcherState = default;

    protected readonly TroopScreenCanvasController _screenCanvasController;

    public TroopBaseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
    {
        _repositoryManager = repositoryManager;

        _troopController = troopController;
        _switcherState = switcherState;

        _troopScriptable = troopController.TroopScriptable;
        _screenCanvasController = screenCanvasController;
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    public abstract void Start();

    public abstract void Stop();

    protected abstract void EnableStateIcon();

    protected virtual void SubscribeToEvents() { }

    protected virtual void UnSubscribeFromEvents() { }
}
