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
using App.Scripts.Views;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Troops.TroopScripts
{
    public abstract class TroopController : MonoBehaviour, IDisposable, IDamagable, IReactableForDamage, ICoroutineRunner
    {
        [SerializeField] protected Transform _bulletInitialPoint;
        [SerializeField] protected TroopScriptable _troopScriptable;
        
        [SerializeField] protected ScreenCanvasView _screenCanvasView;
        [SerializeField] protected WorldCanvasView _worldCanvasView;
        
        [SerializeField] protected UnitAbilityController _unitAbilityController;
        [SerializeField] protected BaseTroopModelController _troopModelController;
        [SerializeField] protected TroopAnimationController _animationController;
        
        [SerializeField] protected StateIconView StateIconView;

        public Transform BulletInitialPoint => _bulletInitialPoint;
        public BaseTroopModelController TroopModelController => _troopModelController;
        
        
        public ScreenCanvasPresenter ScreenPresenter { get; protected set; }
        public WorldCanvasPresenter WorldPresenter { get; protected set; }
        public StatePresenter StatePresenter { get; protected set; }
        public UICanvasMediator<TroopController> UICanvasMediator { get; protected set; }
        
        
        public TroopStatsController StatsController { get; protected set; }
        public TroopStateController StateController { get; protected set; }
        public UnitHealthComponent<TroopController> HealthComponent { get; protected set; }

        public abstract Faction TroopSide { get; }

        protected event Action OnNotificationForGettingDamaged;

        protected TargetSearchService _targetSearchService;
        protected GameEventBus _gameEventBus;
        
        #region Events & Interface Implemention

        protected virtual void OnEnable()
        {
            _gameEventBus.TroopSpawned(this, TroopSide);
        } 

        protected virtual void OnDisable()
        {
            _gameEventBus.TroopDied(this, TroopSide);
        } 
        
        public void Dispose() //
        {
            UICanvasMediator.Dispose();
            StateController.Dispose();
        }

        public void TakeDamage(int attackDamage)
        {
            bool underDefense = StateController.CheckStateForActivity<TroopDefenseState>();
            HealthComponent.TakeDamage(attackDamage, underDefense);
            
            OnNotificationForGettingDamaged?.Invoke(); //
            _worldCanvasView.PlayDamageEffect(); //
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