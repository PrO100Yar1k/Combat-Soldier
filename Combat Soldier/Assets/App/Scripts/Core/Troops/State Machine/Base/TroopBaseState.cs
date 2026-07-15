using Assets.App.Scripts;
using System;

public abstract class TroopBaseState : IDisposable
{
    protected readonly RepositoryManager _repositoryManager;

    protected readonly TroopController _troopController;
    protected readonly TroopScriptable _troopScriptable;
    protected readonly ISwitchableState _switcherState;

    protected readonly ITroopAnimator _animatorController;
    protected readonly TroopScreenCanvasController _screenCanvasController;

    public TroopBaseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
    {
        _repositoryManager = repositoryManager;

        _troopController = troopController;
        _switcherState = switcherState;

        _troopScriptable = troopController.TroopScriptable;
        _screenCanvasController = screenCanvasController;

        _animatorController = animatorController;
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    public void Start()
    {
        PlayStateAnimation();
        OnStart();
    }

    public abstract void OnStart();
    public abstract void OnStop();

    protected abstract void PlayStateAnimation();
    protected abstract void EnableStateIcon();

    protected virtual void SubscribeToEvents() { }
    protected virtual void UnSubscribeFromEvents() { }
}
