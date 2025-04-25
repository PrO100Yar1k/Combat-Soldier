
public class PlayerTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        // to do setup correct sequence of scripts

        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(); // to do

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(_troopScriptable, _screenCanvasController);

        HPController.TakeDamage(25); // test
    }
}