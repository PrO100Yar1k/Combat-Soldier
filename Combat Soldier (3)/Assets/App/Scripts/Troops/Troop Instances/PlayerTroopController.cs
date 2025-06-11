
public class PlayerTroopController : TroopController
{
    public TroopVisionController VisionController { get; private set; }

    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this, _screenCanvasController);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);
    }
}