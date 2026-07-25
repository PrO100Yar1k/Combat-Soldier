using Assets.App.Scripts;
using UnityEngine;
using Zenject;

public class EnemyTroopController : TroopController
{
    private PatrolPointProvider _patrolPointProvider;

    [Inject]
    public void Construct(PatrolPointProvider patrolPointProvider)
    {
        _patrolPointProvider = patrolPointProvider;
    }

    public override void InitializeTroop()
    {
        Transform[] transforms = _patrolPointProvider.GetRandomPatrolPoints();

        StateController = new EnemyStateController(_targetSearchService, this, _screenCanvasController, transforms, _animationController);
        UIController = new UICanvasController<TroopController, TroopScriptable>(this, _troopScriptable, _screenCanvasController, _worldCanvasController, _gameEventBus);
        HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);

        _troopModelController.Initialize(this);
    }
}