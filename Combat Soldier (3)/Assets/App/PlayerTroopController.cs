
public class PlayerTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        // to do setup correct sequence of scripts

        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(); // to do

        UIController = new TroopUIController(_troopScriptable, _canvasController, this);
        HPController = new HPController(_troopScriptable, _canvasController);

        HPController.TakeDamage(25); // test
    }
}