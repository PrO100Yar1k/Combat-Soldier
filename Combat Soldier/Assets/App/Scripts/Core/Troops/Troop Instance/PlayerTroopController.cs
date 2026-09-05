using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Views;
using UnityEngine;

namespace App.Scripts.Core.Troops.Troop_Instance
{
    public class PlayerTroopController : TroopController
    {
        [SerializeField] private ChangePlayerStateView _changeStateButton = default;

        public TroopVisionController VisionController { get; private set; }

        #region Events

        protected override void OnEnable()
        {
            OnNotificationForGettingDamaged += NotifyForGettingDamaged;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            OnNotificationForGettingDamaged -= NotifyForGettingDamaged;
            base.OnDisable();
        }

        #endregion

        public override void InitializeTroop()
        {
            StateController = new PlayerStateController(_targetSearchService, this, _screenCanvasController, _animationController);
            VisionController = new TroopVisionController(this, _troopScriptable, _targetSearchService);

            UIController = new UICanvasController<TroopController, TroopScriptable>(this, _troopScriptable, _screenCanvasController, _worldCanvasController, _gameEventBus);
            HPController = new HPTroopController(this, _screenCanvasController, _troopScriptable);

            _changeStateButton.SetupChangeStateButton(StateController as PlayerStateController);

            _troopModelController.Initialize(this);
        }

        private void NotifyForGettingDamaged()
        {
            Debug.Log("Lord, your unit was damaged!");
        }

        public void UpdateReloadingBar(float timeToReload)
        {
            (_screenCanvasController as PlayerScreenCanvasController)?.UpdateReloadingBar(timeToReload);
        }

        public bool GetCanvasActivityState()
        {
            return (_screenCanvasController as PlayerScreenCanvasController).DisableCanvasAfterOrder;
        }
    }
}