using System;
using App.Scripts.Core.Troops.TroopScripts;

namespace App.Scripts.Core.Ability
{
    public class AbilityRuntime
    {
        public StatEffectConfiguration Config { get; }

        public float CurrentCooldown { get; private set; }
        public float CurrentActiveTime { get; private set; }

        public bool IsActive { get; private set; }
        public bool IsReady => CurrentCooldown <= 0 && !IsActive;

        public event Action OnCooldownChanged;
        public event Action OnStateChanged;

        private readonly TroopController _owner;

        public AbilityRuntime(StatEffectConfiguration config, TroopController owner)
        {
            Config = config;
            _owner = owner;
        }

        public void Tick(float deltaTime)
        {
            if (IsActive)
            {
                CurrentActiveTime -= deltaTime;

                if (CurrentActiveTime <= 0)
                {
                    //Deactivate();
                }
            }
            else if (CurrentCooldown > 0)
            {
                CurrentCooldown -= deltaTime;
                if (CurrentCooldown < 0) CurrentCooldown = 0;

                OnCooldownChanged?.Invoke();
            }
        }
        
        /*
        public bool TryActivate()
        {
            if (!IsReady)
                return false;

            IsActive = true;
            CurrentActiveTime = Config.Duration;

            // Застосовуємо ефекти
            for (int i = 0; i < Config.Effects.Count; i++)
            {
                Config.Effects[i].Apply(_owner, this);
            }

            OnStateChanged?.Invoke();

            if (Config.Duration <= 0)
            {
                Deactivate();
            }

            return true;
        }

        private void Deactivate()
        {
            IsActive = false;

            // Знімаємо ефекти
            for (int i = 0; i < Config.Effects.Count; i++)
            {
                Config.Effects[i].Remove(_owner, this);
            }

            // Запускаємо кулдаун
            CurrentCooldown = Config.Cooldown;

            OnStateChanged?.Invoke();
            OnCooldownChanged?.Invoke();
        } */
    }
}