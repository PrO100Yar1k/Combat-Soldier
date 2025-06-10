using UnityEngine;

public class EnemyTroopController : TroopController
{
    [SerializeField] protected TroopModelController _troopModelController = default;
    public TroopModelController TroopModelController => _troopModelController;

    protected override void InitializeTroop()
    {
        StateController = new TroopStateController(this);

        UIController = new TroopUIController(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPController(this, _screenCanvasController, _troopScriptable);

        _troopModelController.InitializeModelController(gameObject);
    }
}
