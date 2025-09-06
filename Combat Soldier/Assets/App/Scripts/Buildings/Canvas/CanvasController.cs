using UnityEngine;

public abstract class CanvasController : MonoBehaviour
{
    public virtual void EnableCanvas()
        => gameObject.SetActive(true);

    public virtual void DisableCanvas()
        => gameObject.SetActive(false);

    public abstract void InitializeCanvas<T>(T controller);

    protected abstract void AssignDefaultCanvasValues();
}
