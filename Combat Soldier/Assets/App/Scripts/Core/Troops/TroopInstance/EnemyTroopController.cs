using App.Scripts.Core.Ability;
using App.Scripts.Core.Canvases.WorldCanvas;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Core.UI;
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
            StateController = new EnemyStateController(_targetSearchService, this, transforms, _animationController);
            
            UICanvasController = new UICanvasMediator<TroopController>(this, _gameEventBus, _screenStatsCanvasView, _worldCanvasView);
            HealthComponent = new UnitHealthComponent<TroopController>(this, StatsController, _screenStatsCanvasView);

            _worldCanvasView.SetupRunner(this);

            WorldCanvasModel worldModel = new WorldCanvasModel(StatsController);
            WorldPresenter = new WorldCanvasPresenter(worldModel, _worldCanvasView);
            WorldPresenter.DisablePresenter();

            
            _unitAbilityController.Initialize(this);
            HealthComponent.Initialize();
            _troopModelController.Initialize(this);
        }
    }
}