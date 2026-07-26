namespace Assets.App.Scripts.Core.Canvases
{
    public class EnemyWorldCanvasController : TroopWorldCanvasController, IViewRangeVisualizer
    {
        public void InsideViewRange()
        {
            const byte alphaColor = 180;
            SetupUnitRange(alphaColor);
        }

        public void OutsideViewRange()
        {
            StopReloading();

            const byte alphaColor = 85;
            SetupUnitRange(alphaColor);
        }
    }
}
