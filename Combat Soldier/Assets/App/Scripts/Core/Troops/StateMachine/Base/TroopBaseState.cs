using System;
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

        public Sprite StateIcon { get; }

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
            
            StateIcon = GetStateIcon();
        }

        public void EnterState()
        {
            SubscribeToEvents();
            Start();
        }

        public void ExitState()
        {
            UnSubscribeFromEvents();
            Stop();
        }

        private Sprite GetStateIcon() //
        {
            return Resources.Load<Sprite>(StateIconLocation);
        }

        public abstract void Start();
        public abstract void Stop();

        protected abstract void PlayStateAnimation();

        protected virtual void SubscribeToEvents() { }
        protected virtual void UnSubscribeFromEvents() { }
    }
}
