using Assets.App.Scripts;

public class EnemyWorldCanvasController : WorldCanvasController, IViewRangeVisualizer
{
    public void InsideViewRange()
    {
        const byte alphaColor = 180;
        SetupUnitRange(alphaColor);
    }

    public void OutsideViewRange()
    {
        const byte alphaColor = 85;
        SetupUnitRange(alphaColor);
    }
}
