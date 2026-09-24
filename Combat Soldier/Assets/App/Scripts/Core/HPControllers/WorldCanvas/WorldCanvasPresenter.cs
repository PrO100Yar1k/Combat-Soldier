using App.Scripts.MVP;

namespace App.Scripts.Core.Canvases.WorldCanvas
{
    public class WorldCanvasPresenter : PresenterBase<WorldCanvasModel, WorldCanvasView>
    {
        public WorldCanvasPresenter(WorldCanvasModel model, WorldCanvasView view) : base(model, view)
        {
            View.SetupRanges(Model.AttackRange, Model.ViewRange);
        }

        public void TriggerReloading(float time)
        {
            View.StartReloading(time);
        }

        public void TriggerDamageEffect()
        {
            View.PlayDamageEffect();
        }
        
        public void UpdateViewRangeState(bool isInsideRange)
        {
            if (isInsideRange)
                View.SetInsideViewRange();
            else
                View.SetOutsideViewRange();
        }

        public override void EnablePresenter()
        {
            base.EnablePresenter();
            View.SetRangesActive(true);
        }

        public override void DisablePresenter()
        {
            base.DisablePresenter();
            View.SetRangesActive(false);
        }
    }
}