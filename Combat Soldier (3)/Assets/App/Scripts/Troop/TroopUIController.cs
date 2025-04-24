using UnityEngine;

public class TroopUIController 
{
    private readonly TroopCanvasController _canvasController = default;

    public TroopUIController(TroopScriptable troopScriptable, TroopCanvasController canvasController, TroopController troopController)
    {
        _canvasController = canvasController;

        _canvasController.InitializeTroopCanvas(troopScriptable, troopController);

        ChangeCanvasActivationState(false);

        //_canvasController.SetupCircleRanges();
    }

    public void OpenTroopActionMenu()
    {
        ChangeCanvasActivationState(true);

        Debug.Log("Main menu has been opened");
    }

    public void Attack()
    {
        Debug.Log("Attacked");

        // canvascontrololer open attack menu ?
    }


    public void ChangeCanvasActivationState(bool state)
        => _canvasController.gameObject.SetActive(state);
}
