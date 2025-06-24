using System;
using UnityEngine;

public class UICanvasController<T>
{
    private readonly CanvasController _screenCanvasController = default;
    private readonly CanvasController _worldCanvasController = default;

    #region Events

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopDisableCanvases += DisableAllCanvases;

        GameEvents.instance.OnTroopDied += DisableObject;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopDisableCanvases -= DisableAllCanvases;

        GameEvents.instance.OnTroopDied -= DisableObject;
    }

    #endregion

    public UICanvasController(T controller, CanvasController screenCanvasController, CanvasController worldCanvasController) 
    {
        _screenCanvasController = screenCanvasController;
        _worldCanvasController = worldCanvasController;

        _screenCanvasController.InitializeCanvas(controller);
        _worldCanvasController.InitializeCanvas(controller);

        DisableAllCanvases();
    }

    public void OpenTroopGeneralMenu() // to do
    {
        EnableAllCanvases();
    }

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

    private void DisableObject(TroopController troopController, TroopSide troopSide)
    {
        DisableAllCanvases(); // disable and unsubscribe
    }
}
