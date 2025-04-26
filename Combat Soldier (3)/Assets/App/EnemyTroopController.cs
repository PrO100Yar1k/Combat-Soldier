
public class EnemyTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this);
        //VisionController = new TroopVisionController(); // to do

        //UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(_troopScriptable, _screenCanvasController);

        HPController.TakeDamage(55); // test
    }
}
