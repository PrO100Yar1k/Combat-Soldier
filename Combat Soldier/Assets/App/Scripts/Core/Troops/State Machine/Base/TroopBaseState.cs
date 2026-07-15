using System;
using UnityEngine;
using Assets.App.Scripts;

public abstract class TroopBaseState : IDisposable
{
    protected readonly RepositoryManager _repositoryManager;

    protected readonly TroopController _troopController;
    protected readonly TroopScriptable _troopScriptable;
    protected readonly ISwitchableState _switcherState;

    protected readonly ITroopAnimator _animatorController;
    protected readonly TroopScreenCanvasController _screenCanvasController;

    protected abstract string StateIconLocation { get; }

    #region Disposable

    public void Dispose()
    {
        UnSubscribeFromEvents();
    }

    #endregion

    public TroopBaseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
    {
        _repositoryManager = repositoryManager;

        _troopController = troopController;
        _switcherState = switcherState;

        _troopScriptable = troopController.TroopScriptable;
        _screenCanvasController = screenCanvasController;

        _animatorController = animatorController;
    }

    public void Start()
    {
        SubscribeToEvents();
        PlayStateAnimation();
        EnableStateIcon();

        OnStart();
    }

    public void Stop()
    {
        UnSubscribeFromEvents();
        OnStop();
    }

    protected void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>(StateIconLocation);

        if (targetIcon == null)
            return;

        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    public abstract void OnStart();
    public abstract void OnStop();

    protected abstract void PlayStateAnimation();

    protected virtual void SubscribeToEvents() { }
    protected virtual void UnSubscribeFromEvents() { }
}
