using App.Scripts.Core.Ability;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Canvases.WorldCanvas;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Core.UI;
using App.Scripts.Views;
using UnityEngine;

namespace App.Scripts.Core.Troops.TroopInstance
{
    public class PlayerTroopController : TroopController
    {
        [SerializeField] private ReloadingBarView _reloadingBarView;

        public TroopVisionController VisionController { get; private set; }
        public override Faction TroopSide => Faction.Allies;

        public override void InitializeTroop()
        {
            StatsController = new TroopStatsController(_troopScriptable);
            _unitAbilityController.Initialize(this);

            HealthComponent = new UnitHealthComponent<TroopController>(this, StatsController, _screenCanvasView);
            HealthComponent.Initialize();

            _worldCanvasView.SetupRunner(this);

            WorldCanvasModel worldModel = new WorldCanvasModel(StatsController);
            WorldPresenter = new WorldCanvasPresenter(worldModel, _worldCanvasView);
            
            StateController = new PlayerStateController(_targetSearchService, this, _animationController);
            StatePresenter = new StatePresenter(StateController, StateIconView);

            ScreenPresenter = HealthComponent.CanvasPresenter;

            UICanvasMediator = new UICanvasMediator<TroopController>(this, _gameEventBus, ScreenPresenter, WorldPresenter);
            VisionController = new TroopVisionController(this, _troopScriptable, _targetSearchService);
            
            _troopModelController.Initialize(this);
            StateController.Initialize();
        }
        
        public void UpdateReloadingBar(float timeToReload)
        {
            _reloadingBarView.UpdateReloadingBar(timeToReload);
        }
    }
}