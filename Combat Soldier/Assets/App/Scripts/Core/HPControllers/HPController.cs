using UnityEngine;

namespace Assets.App.Scripts.Core.Health 
{
    public abstract class HPController<TData> where TData : ScriptableObject
    {
        protected string _unitName = default;

        protected int _currentHealPoint = default;

        protected HPController(TData config)
        {
            InitializeData(config);
        }

        public abstract void TakeDamage(int attackDamage);

        protected abstract void InitializeData(TData config);

        protected abstract void UpdateSliderAndTextValues();

        protected void CheckHealPointsForDeath()
        {
            if (_currentHealPoint > 0)
                return;

            HandleDeath();
        }

        protected abstract void HandleDeath();
    }
}
