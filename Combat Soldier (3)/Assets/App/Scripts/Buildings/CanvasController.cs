using System;
using UnityEngine;

public abstract class CanvasController : MonoBehaviour
{
    protected Canvas _canvasComponent = default;

    public abstract void InitializeCanvas<T>(T controller);

    public virtual void EnableCanvas()
    {
        _canvasComponent.enabled = true;
    }

    public virtual void DisableCanvas()
    {
        _canvasComponent.enabled = false;
    }

    protected abstract void AssignDefaultCanvasValues();

    protected virtual void Awake()
        => _canvasComponent = GetComponent<Canvas>();
}
