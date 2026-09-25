using System;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class UnitHealthComponent<T> : IDisposable where T : MonoBehaviour
    {
        public ScreenCanvasPresenter CanvasPresenter { get; private set; }
        public HealthModel Health { get; private set; }
        public DefenseModel Defense { get; private set; }

        public event Action OnDamagedVisualEffect; //
        public event Action OnUnitDied; //
        
        private readonly T _controller;
        private readonly IStatsController _statsController;
        private readonly ScreenCanvasView _screenCanvasView;
        
        private IDamageHandler _damageChain;

        public UnitHealthComponent(T controller, IStatsController statsController, ScreenCanvasView screenCanvasView)
        {
            _controller = controller;
            _statsController = statsController;
            _screenCanvasView = screenCanvasView;
        }

        public void Initialize()
        {
            int maxHealth = _statsController.GetStatValueInt(StatType.MaxHealPoint);
            int maxDefense = _statsController.GetStatValueInt(StatType.MaxDefensePoint);
            float blockRate = _statsController.GetStatValueFloat(StatType.BlockRate);
                
            int initialHealth = maxHealth;
            int initialDefense = maxDefense;
            
            Health = new HealthModel(initialHealth, maxHealth);
            Defense = new DefenseModel(initialDefense, maxDefense);

            CanvasPresenter = new ScreenCanvasPresenter(Health, Defense, _screenCanvasView);

            var healthHandler = new HealthDamageHandler(Health);

            if (Defense != null && blockRate > 0)
            {
                var defenseHandler = new DefenseStateDamageHandler(Defense, blockRate);
                defenseHandler.SetNext(healthHandler);
                _damageChain = defenseHandler;
            }
            else
            {
                _damageChain = healthHandler;
            }
            
            Health.OnDied += HandleDeath;
        }

        public void TakeDamage(int attackDamage, bool isDefenseStateEnabled)
        {
            if (attackDamage <= 0 || Health.IsDead)
                return;
            
            if (_damageChain == null)
            {
                Debug.LogError("DamageChain is not initialized yet!");
                return;
            }

            var context = new DamageContext(attackDamage, isDefenseStateEnabled);
            _damageChain?.Handle(context);

            OnDamagedVisualEffect?.Invoke();
        }
        
        private void HandleDeath()
        {
            OnUnitDied?.Invoke();
            Dispose();

            if (_controller == null)
                return;
            
            if (_controller.gameObject == null)
                return;
            
            UnityEngine.Object.Destroy(_controller.gameObject);
        }

        public void Dispose()
        {
            Health.OnDied -= HandleDeath;
            CanvasPresenter?.Dispose();
        }
    }
}