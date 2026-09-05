using App.Scripts.Core.Ability;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Troops.Troop_Instance
{
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
            StatsController = new StatsController(TroopScriptable, _aiPath);

            _troopModelController.Initialize(this);
        }
    }
}