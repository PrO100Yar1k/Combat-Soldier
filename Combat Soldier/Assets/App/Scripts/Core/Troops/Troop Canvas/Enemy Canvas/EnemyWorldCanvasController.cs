using Assets.App.Scripts;
using UnityEngine;

public class EnemyWorldCanvasController : WorldCanvasController, IViewRangeVisualizer
{
    public void InsideViewRange()
    {
        const byte alphaColor = 220;
        SetupUnitRange(alphaColor);
    }

    public void OutsideViewRange()
    {
        const byte alphaColor = 85;
        SetupUnitRange(alphaColor);
    }

    private void SetupUnitRange(byte alphaColor)
    {
        Color32 currentColor = _unitCircleRange.color;
        _unitCircleRange.color = new Color32(currentColor.r, currentColor.g, currentColor.b, alphaColor);
    }
}
