using UnityEngine;

public abstract class CanvasController : MonoBehaviour
{
    protected TroopController _troopController = default;
    protected TroopScriptable _troopScriptable = default;

    protected Canvas _canvasComponent = default;

    public abstract void InitializeCanvas(TroopController troopController);

    public abstract void EnableCanvas();
    public abstract void DisableCanvas();

    protected virtual void Awake()
        => _canvasComponent = GetComponent<Canvas>();
}