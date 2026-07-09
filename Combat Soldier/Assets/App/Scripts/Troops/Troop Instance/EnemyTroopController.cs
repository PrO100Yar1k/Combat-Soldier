using UnityEngine;

public class EnemyTroopController : TroopController
{
    [SerializeField] private MeshRenderer _enemyMeshRendererModel = default;

    public TroopModelController TroopModelController { get; private set; }

    protected override void InitializeTroop()
    {
        const int defaultPointsCount = 4;
        Transform[] transforms = _repositoryManager.GetRandomEnemyPatrollingPoints(defaultPointsCount);

        StateController = new EnemyTroopStateController(_repositoryManager, this, _screenCanvasController, transforms);
        TroopModelController = new TroopModelController(this, gameObject, _enemyMeshRendererModel);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController, _gameEvents);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);
    }
}