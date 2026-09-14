using System;
using UnityEngine;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.TroopScripts
{
    public class UICanvasController<TTarget> : IDisposable where TTarget : MonoBehaviour
    {
        private readonly IInitializableCanvas _screenCanvasController;
        private readonly IInitializableCanvas _worldCanvasController;

        private readonly MonoBehaviour _currentController;
        private readonly GameEventBus _gameEventBus;

        private bool _isSubscribedToActiveEvents;

        #region Events

        private void SubscribeToBasicEvent()
        {
            _gameEventBus.OnOpenTroopMenu += OpeningTroopMenu;
        }

        private void SubscribeToEvents()
        {
            if (_isSubscribedToActiveEvents)
                Debug.LogError("Already have subscription!");

            _gameEventBus.OnDisableActiveCanvases += DisableAllCanvases;

            _gameEventBus.OnBuildingDestroyed += ClosingTroopMenu;
            _gameEventBus.OnTroopDisableUI += ClosingTroopMenu;
            _gameEventBus.OnTroopDiedUI += ClosingTroopMenu;

            _isSubscribedToActiveEvents = true;
        }

        private void UnSubscribeFromEvents()
        {
            _gameEventBus.OnDisableActiveCanvases -= DisableAllCanvases;

            _gameEventBus.OnBuildingDestroyed -= ClosingTroopMenu;
            _gameEventBus.OnTroopDisableUI -= ClosingTroopMenu;
            _gameEventBus.OnTroopDiedUI -= ClosingTroopMenu;

            _isSubscribedToActiveEvents = false;
        }

        public void Dispose()
        {
            UnSubscribeFromEvents();
        }

        #endregion

        public UICanvasController(TTarget controller, IStatsController statsController, IInitializableCanvas screenCanvasController, IInitializableCanvas worldCanvasController, GameEventBus gameEventBus) 
        {
            _currentController = controller;
            _gameEventBus = gameEventBus;

            _screenCanvasController = screenCanvasController;
            _worldCanvasController = worldCanvasController;

            _screenCanvasController?.Initialize(statsController);
            _worldCanvasController?.Initialize(statsController);

            SetupCoroutineRunner();

            SubscribeToBasicEvent();
            DisableAllCanvases();
        }

        public void ChangeUnitCircle(bool isTroopInsideViewRange)
        {
            if (_worldCanvasController is IViewRangeVisualizer visualizer)
            {
                if (isTroopInsideViewRange)
                    visualizer.InsideViewRange();
                else
                    visualizer.OutsideViewRange();
            }
        }

        private void SetupCoroutineRunner()
        {
            if (_currentController is not ICoroutineRunner runner)
                return;

            if (_screenCanvasController is ICoroutineCanvas screenCanvas)
                screenCanvas.SetupCoroutineRunner(runner);

            if (_worldCanvasController is ICoroutineCanvas worldCanvas)
                worldCanvas.SetupCoroutineRunner(runner);
        }

        private void EnableAllCanvases()
        {
            _screenCanvasController?.EnableCanvas();
            _worldCanvasController?.EnableCanvas();

            SubscribeToEvents();
        }

        private void DisableAllCanvases()
        {
            _screenCanvasController?.DisableCanvas();
            _worldCanvasController?.DisableCanvas();

            UnSubscribeFromEvents();
        }

        private void OpeningTroopMenu(MonoBehaviour controller)
        {
            if (_currentController != controller)
                return;

            EnableAllCanvases();
        }

        private void ClosingTroopMenu(MonoBehaviour controller)
        {
            if (_currentController != controller)
                return;

            DisableAllCanvases();
        }
    }
}
