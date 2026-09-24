using App.Scripts.MVP;

namespace App.Scripts.Core.HPControllers
{
    public class ScreenStatsCanvasPresenter : PresenterBase<HealthModel, ScreenStatsCanvasView>
    {
        private readonly DefenseModel _defenseModel;

        public ScreenStatsCanvasPresenter(HealthModel healthModel, DefenseModel defenseModel, ScreenStatsCanvasView canvasView) 
            : base(healthModel, canvasView)
        {
            _defenseModel = defenseModel;

            Model.OnHealthChanged += HandleHealthChanged;
            View.UpdateHealth(Model.CurrentHealth, Model.MaxHealth);

            _defenseModel.OnDefenseChanged += HandleDefenseChanged;
            View.UpdateDefense(_defenseModel.CurrentDefense, _defenseModel.MaxDefense);
        }

        private void HandleHealthChanged(int current, int max)
        {
            View.UpdateHealth(current, max);
        }

        private void HandleDefenseChanged(int current, int max)
        {
            View.UpdateDefense(current, max);
        }

        public new void Dispose()
        {
            Model.OnHealthChanged -= HandleHealthChanged;
            _defenseModel.OnDefenseChanged -= HandleDefenseChanged;
            
            base.Dispose();
        }
    }
}