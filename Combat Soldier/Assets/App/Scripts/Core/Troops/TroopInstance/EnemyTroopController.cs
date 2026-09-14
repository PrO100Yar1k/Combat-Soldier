using App.Scripts.Core.Ability;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Troops.TroopInstance
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

            StatsController = new TroopStatsController(_troopScriptable);
            _unitAbilityController.Initialize(this);

            StateController = new EnemyStateController(_targetSearchService, this, _screenCanvasController, transforms, _animationController);
            
            UIController = new UICanvasController<TroopController>(this, StatsController, _screenCanvasController, _worldCanvasController, _gameEventBus);
            HPController = new HPTroopController(this, _screenCanvasController);

            _troopModelController.Initialize(this);
        }
    }
}