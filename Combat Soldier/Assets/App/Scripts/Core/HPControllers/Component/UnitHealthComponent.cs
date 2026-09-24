using System;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class UnitHealthComponent<T> : IDamageable, IDisposable where T : MonoBehaviour // rename to stats
    {
        public HealthModel Health { get; private set; }
        public DefenseModel Defense { get; private set; }

        public event Action OnDamagedVisualEffect;
        public event Action OnUnitDied;

        private readonly T _controller;
        private readonly IStatsController _statsController;
        private readonly ScreenStatsCanvasView _screenStatsCanvasView;
        
        private ScreenStatsCanvasPresenter canvasPresenter;
        private IDamageHandler _damageChain;

        public UnitHealthComponent(T controller, IStatsController statsController, ScreenStatsCanvasView screenStatsCanvasView)
        {
            _controller = controller;
            _statsController = statsController;
            _screenStatsCanvasView = screenStatsCanvasView;
        }

        public void Initialize()
        {
            int maxHealth = _statsController.GetStatValueInt(StatType.MaxHealPoint);
            int maxDefense = _statsController.GetStatValueInt(StatType.MaxDefensePoint);
            float blockRate = _statsController.GetStatValueInt(StatType.BlockRate);
                
            int initialHealth = maxHealth;
            int initialDefense = maxDefense;
            
            Health = new HealthModel(initialHealth, maxHealth);
            Defense = new DefenseModel(initialDefense, maxDefense);

            canvasPresenter = new ScreenStatsCanvasPresenter(Health, Defense, _screenStatsCanvasView);
            canvasPresenter.DisablePresenter();

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

        public void TakeDamage(int attackDamage, bool isInDefenseState = false)
        {
            if (attackDamage <= 0 || Health.IsDead)
                return;

            var context = new DamageContext(attackDamage, isInDefenseState);
            _damageChain?.Handle(context);

            OnDamagedVisualEffect?.Invoke();
        }

        public void TakeDamage(int amount)
        {
            TakeDamage(amount, false);
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
            canvasPresenter?.Dispose();
        }
    }
}