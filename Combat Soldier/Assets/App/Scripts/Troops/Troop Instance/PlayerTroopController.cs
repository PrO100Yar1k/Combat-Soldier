using UnityEngine;

public class PlayerTroopController : TroopController
{
    public TroopVisionController VisionController { get; private set; }

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

    public override void InitializeTroop()
    {
        StateController = new PlayerTroopStateController(_repositoryManager, this, _screenCanvasController);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController, _gameEvents);
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