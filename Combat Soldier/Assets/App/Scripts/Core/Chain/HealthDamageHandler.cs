namespace App.Scripts.Core.HPControllers
{
    public class HealthDamageHandler : BaseDamageHandler
    {
        private readonly HealthModel _healthModel;

        public HealthDamageHandler(HealthModel healthModel)
        {
            _healthModel = healthModel;
        }

        public override void Handle(DamageContext context)
        {
            if (context.DamageToHealth > 0)
            {
                _healthModel.TakeDamage(context.DamageToHealth);
            }

            base.Handle(context);
        }
    }
}