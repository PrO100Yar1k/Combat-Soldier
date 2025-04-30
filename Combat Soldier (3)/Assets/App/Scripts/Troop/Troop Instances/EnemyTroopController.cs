
public class EnemyTroopController : TroopController
{
    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(this, _troopScriptable, _troopSide);

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(this, _screenCanvasController, _troopScriptable);

        TroopModelController.InitializeModelController();
    }
}
