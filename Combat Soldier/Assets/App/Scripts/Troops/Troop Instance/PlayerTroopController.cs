using UnityEngine;

public class PlayerTroopController : TroopController
{
    public TroopVisionController VisionController { get; private set; } //

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

    protected override void InitializeTroop()
    {
        StateController = new PlayerTroopStateController(this, _screenCanvasController);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);
    }

    private void NotifyForGettingDamaged()
    {
        Debug.Log("Lord, your unit was damaged!");
    }

    public void ScreenCanvasUpdateReloadingBar(float timeToReload)
        => (_screenCanvasController as PlayerScreenCanvasController)?.UpdateReloadingBar(timeToReload);

    public bool GetCanvasActivityStateAfterOrder()
        => (_screenCanvasController as PlayerScreenCanvasController).DisableCanvasAfterOrder;
}