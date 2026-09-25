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

        public override Faction TroopSide => Faction.Enemies;

        [Inject]
        public void Construct(PatrolPointProvider patrolPointProvider)
        {
            _patrolPointProvider = patrolPointProvider;
        }
        
        public override void InitializeTroop()
        {
            StatsController = new TroopStatsController(_troopScriptable);
            _unitAbilityController.Initialize(this);

            HealthComponent = new UnitHealthComponent<TroopController>(this, StatsController, _screenCanvasView);
            HealthComponent.Initialize();

            _worldCanvasView.SetupRunner(this);

            WorldCanvasModel worldModel = new WorldCanvasModel(StatsController);
            WorldPresenter = new WorldCanvasPresenter(worldModel, _worldCanvasView);
            
            Transform[] transforms = _patrolPointProvider.GetRandomPatrolPoints();
            StateController = new EnemyStateController(_targetSearchService, this, transforms, _animationController);
            StatePresenter = new StatePresenter(StateController, StateIconView);
            
            ScreenPresenter = HealthComponent.CanvasPresenter;

            UICanvasMediator = new UICanvasMediator<TroopController>(this, _gameEventBus, ScreenPresenter, WorldPresenter);
            
            _troopModelController.Initialize(this);
            StateController.Initialize();
        }
    }
}