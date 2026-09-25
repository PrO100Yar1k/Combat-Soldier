using App.Scripts.MVP;

namespace App.Scripts.Core.HPControllers
{
    public class ScreenCanvasPresenter : PresenterBase<HealthModel, ScreenCanvasView>
    {
        private readonly DefenseModel _defenseModel;

        public ScreenCanvasPresenter(HealthModel healthModel, DefenseModel defenseModel, ScreenCanvasView screenCanvasView) 
            : base(healthModel, screenCanvasView)
        {
            _defenseModel = defenseModel;

            Model.OnHealthChanged += HandleHealthChanged;
            _defenseModel.OnDefenseChanged += HandleDefenseChanged;
            
            InitializeHealthBar(Model.CurrentHealth, Model.MaxHealth);
            InitializeDefenseBar(_defenseModel.CurrentDefense, _defenseModel.MaxDefense);
            
            View.UpdateHealth(Model.CurrentHealth, Model.MaxHealth);
            View.UpdateDefense(_defenseModel.CurrentDefense, _defenseModel.MaxDefense);
        }
        
        private void InitializeHealthBar(int currentHealth, int maxHealth)
        {
            View.InitializeHealthBar(currentHealth, maxHealth);
        }
        
        private void InitializeDefenseBar(int currentDefense, int maxDefense)
        {
            View.InitializeDefenseBar(currentDefense, maxDefense);
        }

        private void HandleHealthChanged(int currentHealth, int maxHealth)
        {
            View.UpdateHealth(currentHealth, maxHealth);
        }

        private void HandleDefenseChanged(int currentDefense, int maxDefense)
        {
            View.UpdateDefense(currentDefense, maxDefense);
        }

        public override void Dispose()
        {
            Model.OnHealthChanged -= HandleHealthChanged;
            _defenseModel.OnDefenseChanged -= HandleDefenseChanged;
            
            base.Dispose();
        }
    }
}