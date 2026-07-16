using UnityEngine;

public class EnemyTroopController : TroopController
{
    public override void InitializeTroop()
    {
        Transform[] transforms = _repositoryManager.GetRandomEnemyPatrollingPoints();

        StateController = new EnemyStateController(_repositoryManager, this, _screenCanvasController, transforms, _animationController);
        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController, _gameEventBus);
        HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);

        _troopModelController.Initialize(this);
    }
}