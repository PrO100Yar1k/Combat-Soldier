using UnityEngine;

public class TroopUIController 
{
    private readonly TroopCanvasController _canvasController = default;

    public TroopUIController(TroopScriptable troopScriptable, TroopCanvasController canvasController, TroopController troopController)
    {
        _canvasController = canvasController;

        _canvasController.InitializeCanvas(troopScriptable, troopController);

        ChangeCanvasActivationState(false);
    }

    public void OpenTroopActionMenu()
    {
        ChangeCanvasActivationState(true);

        _canvasController.ChangeCirclesState(true);

        Debug.Log("Main menu has been opened");
    }

    public void OpenAttackMenu()
    {
        Debug.Log("Attack Menu Opened");

        // canvascontrololer open attack menu ?
    }


    public void ChangeCanvasActivationState(bool state)
        => _canvasController.gameObject.SetActive(state);
}
