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
        GameEvents.instance.OnTroopDiedSimple += DisableObject;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnDisableActiveCanvases -= DisableAllCanvases;

        GameEvents.instance.OnBuildingDestroyed -= DisableObject;
        GameEvents.instance.OnTroopDiedSimple -= DisableObject;
    }

    #endregion

    public UICanvasController(T controller, CanvasController screenCanvasController, CanvasController worldCanvasController) 
    {
        _currentController = controller;

        _screenCanvasController = screenCanvasController;
        _worldCanvasController = worldCanvasController;

        _screenCanvasController.InitializeCanvas(controller);
        _worldCanvasController.InitializeCanvas(controller);

        DisableAllCanvases();
    }

    public void Dispose()
        => UnSubscribeFromEvents();

    public void OpenTroopGeneralMenu()
        => EnableAllCanvases();

    public void OpenAttackMenu() // to do
    {
        Debug.Log("Attack menu opened");
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
        if (controller == _currentController)
            DisableAllCanvases();
    }
}
