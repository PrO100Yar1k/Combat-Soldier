
public class PlayerTroopController : TroopController
{
    public TroopVisionController VisionController { get; private set; }

    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(this, _screenCanvasController, _troopScriptable);

        //HPController.TakeDamage(25);
    }
}