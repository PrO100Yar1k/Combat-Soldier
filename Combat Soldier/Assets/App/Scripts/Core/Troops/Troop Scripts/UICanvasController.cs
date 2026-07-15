using System;
using UnityEngine;

public class UICanvasController<T> : IDisposable where T : MonoBehaviour
{
    private readonly CanvasController _screenCanvasController = default;
    private readonly CanvasController _worldCanvasController = default;

    private readonly MonoBehaviour _currentController = default;

    private GameEventBus _gameEventBus = default;

    #region Events

    private void SubscribeToEvents()
    {
        _gameEventBus.OnDisableActiveCanvases += DisableAllCanvases;

        _gameEventBus.OnBuildingDestroyed += DisableObject;
        _gameEventBus.OnTroopDisableUI += DisableObject;
        _gameEventBus.OnTroopDiedUI += DisableObject;
    }

    private void UnSubscribeFromEvents()
    {
        _gameEventBus.OnDisableActiveCanvases -= DisableAllCanvases;

        _gameEventBus.OnBuildingDestroyed -= DisableObject;
        _gameEventBus.OnTroopDisableUI -= DisableObject;
        _gameEventBus.OnTroopDiedUI -= DisableObject;
    }

    private void SubscribeToBasicEvent()
    {
        _gameEventBus.OnOpenTroopMenu += OpenTroopGeneralMenu;
    }

    public void Dispose()
    {
        UnSubscribeFromEvents();
    }

    #endregion

    public UICanvasController(T controller, CanvasController screenCanvasController, CanvasController worldCanvasController, GameEventBus gameEventBus) 
    {
        _gameEventBus = gameEventBus;

        _currentController = controller;

        _screenCanvasController = screenCanvasController;
        _worldCanvasController = worldCanvasController;

        _screenCanvasController.InitializeCanvas(controller);
        _worldCanvasController.InitializeCanvas(controller);

        SubscribeToBasicEvent();
        DisableAllCanvases();
    }

    private void OpenTroopGeneralMenu(MonoBehaviour controller)
    {
        if (_currentController != controller)
            return;

        EnableAllCanvases();
    }

    private void EnableAllCanvases()
    {
        _screenCanvasController.EnableCanvas();
        _worldCanvasController.EnableCanvas();

        SubscribeToEvents();
    }

    private void DisableAllCanvases()
    {
        _screenCanvasController.DisableCanvas();
        _worldCanvasController.DisableCanvas();

        UnSubscribeFromEvents();
    }

    private void DisableObject(MonoBehaviour controller)
    {
        if (_currentController != controller)
            return;

        DisableAllCanvases();
    }
}
