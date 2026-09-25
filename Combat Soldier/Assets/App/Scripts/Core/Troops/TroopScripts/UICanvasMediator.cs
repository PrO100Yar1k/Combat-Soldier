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

        private readonly IPresenter _screenPresenter;
        private readonly IPresenter _worldPresenter;

        private bool _isSubscribedToActiveEvents;
        
        public bool DeactivateCanvasAfterOrder => true;

        public UICanvasMediator(TTarget unitController, GameEventBus gameEventBus, IPresenter screenPresenter, IPresenter worldPresenter)
        {
            _unitController = unitController;
            _gameEventBus = gameEventBus;

            _screenPresenter = screenPresenter;
            _worldPresenter = worldPresenter;

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
            _screenPresenter?.EnablePresenter();
            _worldPresenter?.EnablePresenter();

            SubscribeToActiveEvents();
        }

        public void HideAllViews()
        {
            _screenPresenter?.DisablePresenter();
            _worldPresenter?.DisablePresenter();

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