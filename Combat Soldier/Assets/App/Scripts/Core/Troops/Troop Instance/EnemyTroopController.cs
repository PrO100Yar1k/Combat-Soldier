using UnityEngine;

public class EnemyTroopController : TroopController
{
    [SerializeField] private MeshRenderer _enemyMeshRendererModel = default;

    public TroopModelController TroopModelController { get; private set; }

    public override void InitializeTroop()
    {
        Transform[] transforms = _repositoryManager.GetRandomEnemyPatrollingPoints();

        StateController = new EnemyStateController(_repositoryManager, this, _screenCanvasController, transforms, _animationController);
        TroopModelController = new TroopModelController(this, gameObject, _enemyMeshRendererModel, _gameEventBus);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController, _gameEventBus);
        HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);
    }
}