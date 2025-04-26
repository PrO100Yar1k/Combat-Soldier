
public class EnemyTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this);
        //VisionController = new TroopVisionController(); // to do

        //UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(this, _screenCanvasController, _troopScriptable);

        //HPController.TakeDamage(55); // test
    }
}
