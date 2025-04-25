using UnityEngine;

public abstract class CanvasController : MonoBehaviour
{
    protected TroopScriptable _troopScriptable = default;
    protected TroopController _troopController = default;

    public abstract void InitializeCanvas(TroopScriptable troopScriptable, TroopController troopController);

    protected abstract void EnableCanvas();
    protected abstract void DisableCanvas();
}