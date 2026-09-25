using System;
using App.Scripts.MVP;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class HealthModel : ModelBase // , IDamagable, IHealable
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public bool IsDead => CurrentHealth <= 0;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public HealthModel(int initialHealth, int maxHealth)
        {
            CurrentHealth = Mathf.Clamp(initialHealth, 0, maxHealth);
            MaxHealth = Mathf.Max(1, maxHealth);
        }
    
        public override void ResetState()
        {
            SetHealth(MaxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0 || IsDead)
                return;

            SetHealth(CurrentHealth - amount);
        }

        public void Heal(int amount)
        {
            if (amount <= 0 || IsDead)
                return;

            SetHealth(CurrentHealth + amount);
        }

        private void SetHealth(int newHealth)
        {
            int clampedHealth = Mathf.Clamp(newHealth, 0, MaxHealth);

            if (clampedHealth == CurrentHealth)
                return;

            bool wasAlive = !IsDead;
            CurrentHealth = clampedHealth;

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        
            CheckHealthForDeath(wasAlive);
        }
    
        private void CheckHealthForDeath(bool wasAlive)
        {
            if (!wasAlive || !IsDead)
                return;
        
            OnDied?.Invoke();
        }
    }
}