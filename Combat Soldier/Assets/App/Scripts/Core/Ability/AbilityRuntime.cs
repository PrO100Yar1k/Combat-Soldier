using System;
using App.Scripts.Core.Troops.TroopScripts;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public class AbilityRuntime
    {
        private readonly TroopController _owner;
        private readonly Ability _config;
        
        private float _currentCooldown;
        private float _currentActiveTime;

        public event Action OnCooldownChanged;
        public event Action OnStateChanged;
        
        public bool IsActive { get; private set; }
        public bool IsReady => _currentCooldown <= 0 && !IsActive;
        
        public AbilityRuntime(Ability config, TroopController owner)
        {
            _config = config;
            _owner = owner;
        }

        public void Tick(float deltaTime)
        {
            if (IsActive)
            {
                _currentActiveTime -= deltaTime;

                if (_currentActiveTime <= 0)
                {
                    Deactivate();
                }
            }
            else if (_currentCooldown > 0)
            {
                _currentCooldown -= deltaTime;
                
                if (_currentCooldown < 0) 
                    _currentCooldown = 0;

                OnCooldownChanged?.Invoke();
            }
        }

        public bool TryActivate()
        {
            if (!IsReady)
                return false;

            IsActive = true;
            _currentActiveTime = _config.Duration;

            _config.Apply(_owner, this);

            OnStateChanged?.Invoke();

            return true;
        }

        private void Deactivate()
        {
            IsActive = false;
            _currentActiveTime = 0;

            _config.Remove(_owner, this);

            _currentCooldown = _config.Cooldown;

            OnStateChanged?.Invoke();
            OnCooldownChanged?.Invoke();
            
            Debug.Log("Deactivated");
        }
    }
}