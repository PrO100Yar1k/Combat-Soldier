using UnityEngine;

public class PlayerTroopController : TroopController
{
    [SerializeField] private ChangeTroopState _changeStateButton = default;

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
        StateController = new PlayerStateController(_repositoryManager, this, _screenCanvasController);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController, _gameEventBus);
        HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);

        _changeStateButton.SetupChangeStateButton(StateController as PlayerStateController);
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