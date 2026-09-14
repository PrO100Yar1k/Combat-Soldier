using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Ability;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Canvases.WorldCanvas;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Events;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Buildings.Base
{
    public abstract class BuildingController : MonoBehaviour, IDamagable, IDisposable
    {
        [SerializeField] protected BuildingScriptable _buildingScriptable;

        [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController;
        [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController;

        [SerializeField] protected Transform _observePoint;

        [SerializeField, Space(2)] protected List<GameObject> _rotatingObjectList;
        [SerializeField, Space(2)] protected List<Transform> _bulletInitialPointList;

        public UICanvasController<BuildingController> UIController { get; protected set; }
        public HPBuildingController HPController { get; protected set; }
        public BuildingStatsController StatsController { get; protected set; }

        public BuildingScriptable BuildingScriptable => _buildingScriptable;

        protected BaseBuildingBehaviour _buildingAttack = default;

        private GameEventBus _gameEvents;
        protected ICoroutineRunner _coroutineRunner;
        protected TargetSearchService _targetSearchService;

        #region Events & Interface Implemention

        protected virtual void OnEnable()
        {
            _gameEvents.BuildingSpawned(this);
        }

        protected virtual void OnDisable()
        {
            _gameEvents.BuildingDestroyed(this);
        }

        public void Dispose()
        {
            UIController.Dispose();
        }

        public void TakeDamage(int attackDamage)
        {
            HPController.TakeDamage(attackDamage);
        }

        public void TryExecuteAttack()
        {
            if (_buildingAttack.IsAttacking)
                return;

            _buildingAttack.CheckAndTryToAttackEnemy();
        }

        public Faction GetFaction()
        {
            return Faction.Enemies;
        }

        #endregion

        [Inject]
        public void Construct(GameEventBus gameEvents, TargetSearchService targetSearchService, ICoroutineRunner coroutineRunner)
        {
            _gameEvents = gameEvents;
            _coroutineRunner = coroutineRunner;
            _targetSearchService = targetSearchService;
        }

        public virtual void InitializeBuilding()
        {
            StatsController = new BuildingStatsController(_buildingScriptable);
            UIController = new UICanvasController<BuildingController>(this, StatsController, _buildingScreenCanvasController, _buildingWorldCanvasController, _gameEvents);
            HPController = new HPBuildingController(this, _buildingScreenCanvasController);

            InitializeBuildingBehaviour();
        }

        protected abstract void InitializeBuildingBehaviour();
    }


    public interface IDamagable //
    {
        public void TakeDamage(int attackDamage);
        public Faction GetFaction();
    }

    public interface IAttackable //
    {
        public void Attack(IDamagable attackTarget);
        public IEnumerator CheckAttackTargetCoroutine();
    }
}