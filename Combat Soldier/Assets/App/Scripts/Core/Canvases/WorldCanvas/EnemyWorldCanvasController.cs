using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Canvases.WorldCanvas
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
            DisableReloadingCircles();

            const byte alphaColor = 85;
            SetupUnitRange(alphaColor);
        }

        public void DisableReloadingCircles()
        {
            _unitCircleLining.gameObject.SetActive(false);
            _unitReloadingCircleRange.gameObject.SetActive(false);
        }
    }
}
