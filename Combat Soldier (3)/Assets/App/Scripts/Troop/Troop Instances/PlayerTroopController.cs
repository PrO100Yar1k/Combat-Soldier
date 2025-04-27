
public class PlayerTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        // to do setup correct sequence of scripts

        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(this, _troopScriptable, _troopSide); // to do

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(this, _screenCanvasController, _troopScriptable);

        HPController.TakeDamage(25); // test
    }
}