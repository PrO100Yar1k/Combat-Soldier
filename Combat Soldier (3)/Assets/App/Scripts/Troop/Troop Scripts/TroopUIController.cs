using UnityEngine;

public class TroopUIController 
{
    private readonly ScreenCanvasController _screenCanvasController = default;
    private readonly WorldCanvasController _worldCanvasController = default;

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

    public TroopUIController(TroopController troopController, ScreenCanvasController screenCanvasController, WorldCanvasController worldCanvasController)
    {
        _screenCanvasController = screenCanvasController;
        _worldCanvasController = worldCanvasController;

        _screenCanvasController.InitializeCanvas(troopController);
        _worldCanvasController.InitializeCanvas(troopController);

        DisableAllCanvases();
    }

    public void OpenTroopGeneralMenu()
    {
        EnableAllCanvases();
    }

    public void OpenAttackMenu()
    {
        // to do

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
