using App.Scripts.Core.Ability;
using UnityEngine;

namespace App.Scripts.Core.HPControllers 
{
    public abstract class HPController
    {
        protected string _unitName;
        protected int _currentHealPoint;

        public abstract void TakeDamage(int attackDamage);

        protected abstract void InitializeData(IStatsController statsController);

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
