using UnityEngine;

public abstract class TroopCanvasController : MonoBehaviour
{
    protected TroopController _troopController = default;
    protected TroopScriptable _troopScriptable = default;

    protected Canvas _canvasComponent = default;

    public abstract void InitializeCanvas(TroopController troopController);

    public virtual void EnableCanvas() 
    {
        _canvasComponent.enabled = true;
    }

    public virtual void DisableCanvas()
    {
        _canvasComponent.enabled = false;
    }

    protected virtual void Awake()
        => _canvasComponent = GetComponent<Canvas>();
}