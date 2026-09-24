using System;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.StateMachine.Base
{
    public abstract class TroopBaseState : IDisposable
    {
        protected readonly TargetSearchService _targetSearchService;

        protected readonly TroopController _troopController;
        protected readonly ISwitchableState _switcherState;

        protected readonly ITroopAnimator _animatorController;

        protected abstract string StateIconLocation { get; }

        #region Disposable

        public virtual void Dispose()
        {
            UnSubscribeFromEvents();
        }

        #endregion

        public TroopBaseState(TargetSearchService targetSearchService, TroopController troopController, ISwitchableState switcherState, ITroopAnimator animatorController)
        {
            _switcherState = switcherState;
            _targetSearchService = targetSearchService;

            _troopController = troopController;
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

            //_screenCanvasController.ChangeStateIcon(targetIcon);
        }

        public abstract void OnStart();
        public abstract void OnStop();

        protected abstract void PlayStateAnimation();

        protected virtual void SubscribeToEvents() { }
        protected virtual void UnSubscribeFromEvents() { }
    }
}
