using UnityEngine;

public class EnemyScreenCanvasController : ScreenCanvasController
{
    public override void EnableCanvas()
    {
        _canvasComponent.enabled = true;
    }

    public override void DisableCanvas()
    {
        _canvasComponent.enabled = false;
    }
}
