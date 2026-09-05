using System;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.State_Machine.Base
{
    public abstract class TroopBaseState : IDisposable
    {
        protected readonly TargetSearchService _targetSearchService;

        protected readonly TroopController _troopController;
        protected readonly TroopScriptable _troopScriptable;
        protected readonly ISwitchableState _switcherState;

        protected readonly ITroopAnimator _animatorController;
        protected readonly TroopScreenCanvasController _screenCanvasController;

        protected abstract string StateIconLocation { get; }

        #region Disposable

        public virtual void Dispose()
        {
            UnSubscribeFromEvents();
        }

        #endregion

        public TroopBaseState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        {
            _targetSearchService = targetSearchService;

            _troopController = troopController;
            _switcherState = switcherState;

            _troopScriptable = troopController?.TroopScriptable;
            _screenCanvasController = screenCanvasController;

            _animatorController = animatorController;
        }

        public void Start()
        {
            SubscribeToEvents();
            EnableStateIcon();

            OnStart();
        }

        public void Stop()
        {
            UnSubscribeFromEvents();
            OnStop();
        }

        protected void EnableStateIcon()
        {
            Sprite targetIcon = Resources.Load<Sprite>(StateIconLocation);

            if (targetIcon == null)
                return;

            _screenCanvasController.ChangeStateIcon(targetIcon);
        }

        public abstract void OnStart();
        public abstract void OnStop();

        protected abstract void PlayStateAnimation();

        protected virtual void SubscribeToEvents() { }
        protected virtual void UnSubscribeFromEvents() { }
    }
}
