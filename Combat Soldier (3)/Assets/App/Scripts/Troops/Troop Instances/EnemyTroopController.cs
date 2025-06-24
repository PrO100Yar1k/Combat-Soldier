using UnityEngine;

public class EnemyTroopController : TroopController
{
    [SerializeField, Space(2)] protected TroopModelController _troopModelController = default;
    public TroopModelController TroopModelController => _troopModelController;

    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this, _screenCanvasController);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);

        _troopModelController.InitializeModelController(gameObject);
    }
}