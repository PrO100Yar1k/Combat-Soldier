using System;
using App.Scripts.Infrastructure.Events;
using App.Scripts.MVP;
using UnityEngine;

namespace App.Scripts.Core.UI
{
    public class UICanvasMediator<TTarget> : IDisposable where TTarget : MonoBehaviour
    {
        private readonly TTarget _unitController;
        private readonly GameEventBus _gameEventBus;

        private readonly IView _screenView;
        private readonly IView _worldView;

        private bool _isSubscribedToActiveEvents;

        public UICanvasMediator(TTarget unitController, GameEventBus gameEventBus, IView screenView = null, IView worldView = null)
        {
            _unitController = unitController;
            _gameEventBus = gameEventBus;

            _screenView = screenView;
            _worldView = worldView;

            SubscribeToBasicEvents();
            HideAllViews();
        }
        
        #region Disposable
        
        public void Dispose()
        {
            UnsubscribeFromBasicEvents();
            UnsubscribeFromActiveEvents();
        }
        
        #endregion

        #region Event Subscriptions

        private void SubscribeToBasicEvents()
        {
            _gameEventBus.OnOpenTroopMenu += HandleOpeningTroopMenu;
        }

        private void UnsubscribeFromBasicEvents()
        {
            _gameEventBus.OnOpenTroopMenu -= HandleOpeningTroopMenu;
        }

        private void SubscribeToActiveEvents()
        {
            if (_isSubscribedToActiveEvents)
                return;

            _gameEventBus.OnDisableActiveCanvases += HideAllViews;
            _gameEventBus.OnBuildingDestroyed += HandleClosingTroopMenu;
            _gameEventBus.OnTroopDisableUI += HandleClosingTroopMenu;
            _gameEventBus.OnTroopDiedUI += HandleClosingTroopMenu;

            _isSubscribedToActiveEvents = true;
        }

        private void UnsubscribeFromActiveEvents()
        {
            if (!_isSubscribedToActiveEvents)
                return;

            _gameEventBus.OnDisableActiveCanvases -= HideAllViews;
            _gameEventBus.OnBuildingDestroyed -= HandleClosingTroopMenu;
            _gameEventBus.OnTroopDisableUI -= HandleClosingTroopMenu;
            _gameEventBus.OnTroopDiedUI -= HandleClosingTroopMenu;

            _isSubscribedToActiveEvents = false;
        }

        #endregion

        #region Visibility Management

        public void ShowAllViews()
        {
            _screenView?.Show();
            _worldView?.Show();

            SubscribeToActiveEvents();
        }

        public void HideAllViews()
        {
            _screenView?.Hide();
            _worldView?.Hide();

            UnsubscribeFromActiveEvents();
        }

        private void HandleOpeningTroopMenu(MonoBehaviour controller)
        {
            if (_unitController != controller)
                return;
            
            ShowAllViews();
        }

        private void HandleClosingTroopMenu(MonoBehaviour controller)
        {
            if (_unitController != controller)
                return;
            
            HideAllViews();
        }

        #endregion
    }
}