using System;
using UnityEngine;

public class UICanvasController<T> : IDisposable where T : MonoBehaviour
{
    private readonly CanvasController _screenCanvasController = default;
    private readonly CanvasController _worldCanvasController = default;

    private readonly MonoBehaviour _currentController = default;

    #region Events

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnDisableActiveCanvases += DisableAllCanvases;

        GameEvents.instance.OnBuildingDestroyed += DisableObject;
        GameEvents.instance.OnTroopDisableUI += DisableObject;
        GameEvents.instance.OnTroopDiedUI += DisableObject;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnDisableActiveCanvases -= DisableAllCanvases;

        GameEvents.instance.OnBuildingDestroyed -= DisableObject;
        GameEvents.instance.OnTroopDisableUI -= DisableObject;
        GameEvents.instance.OnTroopDiedUI -= DisableObject;
    }

    private void SubscribeToBasicEvent()
    {
        GameEvents.instance.OnOpenTroopMenu += OpenTroopGeneralMenu;
    }

    #endregion

    public UICanvasController(T controller, CanvasController screenCanvasController, CanvasController worldCanvasController) 
    {
        _currentController = controller;

        _screenCanvasController = screenCanvasController;
        _worldCanvasController = worldCanvasController;

        _screenCanvasController.InitializeCanvas(controller);
        _worldCanvasController.InitializeCanvas(controller);

        SubscribeToBasicEvent();
        DisableAllCanvases();
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    private void OpenTroopGeneralMenu(MonoBehaviour controller)
    {
        if (_currentController != controller)
            return;

        EnableAllCanvases();
    }

    //private void OpenAttackMenu()
    //{
    //    Debug.Log("Attack menu opened");
    //}

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
