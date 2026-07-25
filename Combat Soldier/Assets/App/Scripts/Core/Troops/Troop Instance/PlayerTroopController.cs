using Assets.App.Scripts.Core.Canvases;
using UnityEngine;

public class PlayerTroopController : TroopController
{
    [SerializeField] private ChangePlayerTroopState _changeStateButton = default;

    public TroopVisionController VisionController { get; private set; }

    #region Events

    protected override void OnEnable()
    {
        OnNotificationForGettingDamaged += NotifyForGettingDamaged;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        OnNotificationForGettingDamaged -= NotifyForGettingDamaged;
        base.OnDisable();
    }

    #endregion

    public override void InitializeTroop()
    {
        StateController = new PlayerStateController(_targetSearchService, this, _screenCanvasController, _animationController);
        VisionController = new TroopVisionController(this, _troopScriptable, _targetSearchService);

        UIController = new UICanvasController<TroopController, TroopScriptable>(this, _troopScriptable, _screenCanvasController, _worldCanvasController, _gameEventBus);
        HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);

        _changeStateButton.SetupChangeStateButton(StateController as PlayerStateController);

        _troopModelController.Initialize(this);
    }

    private void NotifyForGettingDamaged()
    {
        Debug.Log("Lord, your unit was damaged!");
    }

    public void UpdateReloadingBar(float timeToReload)
    {
        (_screenCanvasController as PlayerScreenCanvasController)?.UpdateReloadingBar(timeToReload);
    }

    public bool GetCanvasActivityState()
    {
        return (_screenCanvasController as PlayerScreenCanvasController).DisableCanvasAfterOrder;
    }
}