using UnityEngine;

public class TroopUIController 
{
    private readonly TroopCanvasController _canvasController = default;

    public TroopUIController(TroopScriptable troopScriptable, TroopCanvasController canvasController)
    {
        _canvasController = canvasController;

        _canvasController.InitializeTroopCanvas(troopScriptable);

        _canvasController.SetupCircleRanges();
    }

    public void OpenMainMenu()
    {
        Debug.Log("Main menu has been opened");

        // canvascontrololer open it
    }

    public void Attack()
    {
        Debug.Log("Attacked");
        // canvascontrololer open attack menu ?
    }
}
