using System;
using App.Scripts.Core.Ability;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Canvases.WorldCanvas;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.Model;
using App.Scripts.Core.Troops.StateMachine.Attack_State;
using App.Scripts.Core.Troops.StateMachine.Death_State;
using App.Scripts.Core.Troops.StateMachine.Defense_State;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.UI;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Troops.TroopScripts
{
    public abstract class TroopController : MonoBehaviour, IDisposable, IDamagable, IReactableForDamage, ICoroutineRunner
    {
        [SerializeField] protected Transform _bulletInitialPoint;
        [SerializeField] protected TroopScriptable _troopScriptable;

        [SerializeField] protected BaseTroopModelController _troopModelController;
        
        
        [SerializeField] protected ScreenStatsCanvasView _screenStatsCanvasView;
        [SerializeField] protected WorldCanvasView _worldCanvasView;
        
        
        [SerializeField] protected UnitAbilityController _unitAbilityController;
        
        [SerializeField] protected TroopAnimationController _animationController;

        public Transform BulletInitialPoint => _bulletInitialPoint;
        public BaseTroopModelController TroopModelController => _troopModelController;
        public UnitAbilityController UnitAbilityController => _unitAbilityController;
        public TroopScriptable TroopScriptable => _troopScriptable;
        
        public UICanvasMediator<TroopController> UICanvasController { get; protected set; }
        public TroopStateController StateController { get; protected set; }
        public UnitHealthComponent<TroopController> HealthComponent { get; protected set; }
        public WorldCanvasPresenter WorldPresenter { get; protected set; }
        public TroopStatsController StatsController { get; protected set; }

        public Faction TroopSide => _troopScriptable.TroopSide;

        protected event Action OnNotificationForGettingDamaged;

        protected TargetSearchService _targetSearchService;
        protected GameEventBus _gameEventBus;
        
        #region Events & Interface Implemention

        protected virtual void OnEnable() 
            => _gameEventBus.TroopSpawned(this, TroopSide);

        protected virtual void OnDisable()
            => _gameEventBus.TroopDied(this, TroopSide);
        
        public void Dispose()
        {
            UICanvasController.Dispose();
            StateController.Dispose();
        }

        public void TakeDamage(int attackDamage)
        {
            HealthComponent.TakeDamage(attackDamage);
            OnNotificationForGettingDamaged?.Invoke(); //

            _worldCanvasView.PlayDamageEffect();
        }

        public Faction GetFaction()
        {
            return TroopSide;
        }

        public void ChangeUnitCircleToReloading(float reloadingTime)
        {
            _worldCanvasView.StartReloading(reloadingTime);
        }

        #endregion

        [Inject]
        public void Construct(GameEventBus gameEventBus, TargetSearchService targetSearchService)
        {
            _gameEventBus = gameEventBus;
            _targetSearchService = targetSearchService;
        }

        public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
        {
            if (StateController.CheckStateForActivity<TroopAttackState>() || StateController.CheckStateForActivity<TroopDeathState>())
                return;

            Vector3 currentPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float attackRange = _troopScriptable.AttackRangeRadius;

            if (Vector3.Distance(currentPos, targetPos) > attackRange)
                return;

            StateController.ActivateDefenseUnderAttack(target, targetPos);
        }

        public abstract void InitializeTroop();
    }

    public enum Faction //
    {
        None,
        Allies,
        Enemies
    }
}