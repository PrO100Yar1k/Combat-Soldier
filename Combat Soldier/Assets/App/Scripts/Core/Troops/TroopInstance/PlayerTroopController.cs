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
        [SerializeField] private ChangePlayerStateView _changeStateButton;
        [SerializeField] private ReloadingBarView _reloadingBarView;
        
        public TroopVisionController VisionController { get; private set; }
        
        public override void InitializeTroop()
        {
            StatsController = new TroopStatsController(_troopScriptable);
            StateController = new PlayerStateController(_targetSearchService, this, _animationController);
            
            VisionController = new TroopVisionController(this, _troopScriptable, _targetSearchService);

            UICanvasController = new UICanvasMediator<TroopController>(this, _gameEventBus, _screenStatsCanvasView,_worldCanvasView);
            HealthComponent = new UnitHealthComponent<TroopController>(this, StatsController, _screenStatsCanvasView);

            _worldCanvasView.SetupRunner(this);

            WorldCanvasModel worldModel = new WorldCanvasModel(StatsController);
            WorldPresenter = new WorldCanvasPresenter(worldModel, _worldCanvasView);
            WorldPresenter.DisablePresenter();
            
            _unitAbilityController.Initialize(this);
            HealthComponent.Initialize();
            
            _changeStateButton.SetupChangeStateButton(StateController as PlayerStateController);
            _troopModelController.Initialize(this);
        }
        
        public void UpdateReloadingBar(float timeToReload)
        {
            _reloadingBarView?.UpdateReloadingBar(timeToReload);
        }

        public bool GetCanvasActivityState()
        {
            return true;  //(_screenCanvasController as PlayerScreenCanvasController).DisableCanvasAfterOrder;
        }
    }
}