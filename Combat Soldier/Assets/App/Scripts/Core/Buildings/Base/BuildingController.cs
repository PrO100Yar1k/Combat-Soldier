using Assets.App.Scripts.Core.Canvases;
using System.Collections;
using UnityEngine;
using Zenject;
using System;

public abstract class BuildingController : MonoBehaviour, IDamagable, IDisposable
{
    [SerializeField] protected BuildingScriptable _buildingScriptable = default;

    [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController = default;
    [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController = default;

    public UICanvasController<BuildingController, BuildingScriptable> UIController { get; protected set; }
    public HPBuildingController HPController { get; protected set; }

    public BuildingScriptable BuildingScriptable => _buildingScriptable;

    protected BaseBuildingAttack _buildingAttack = default;

    protected GameEventBus _gameEvents = default;
    protected TargetSearchService _targetSearchService = default;

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

    public Faction GetFaction()
    {
        return Faction.Enemies;
    }

    #endregion

    [Inject]
    public void Construct(GameEventBus gameEvents, TargetSearchService targetSearchService)
    {
        _gameEvents = gameEvents;
        _targetSearchService = targetSearchService;
    }

    public virtual void InitializeBuilding()
    {
        UIController = new UICanvasController<BuildingController, BuildingScriptable>(this, _buildingScriptable, _buildingScreenCanvasController, _buildingWorldCanvasController, _gameEvents);
        HPController = new HPBuildingController(this, _buildingScreenCanvasController, _buildingScriptable);

        InitializeBuildingBehaviour();
    }

    protected abstract void InitializeBuildingBehaviour();
}


public interface IDamagable
{
    public void TakeDamage(int attackDamage);
    public Faction GetFaction();
}

public interface IAttackable
{
    public void Attack(IDamagable attackTarget);
    public IEnumerator CheckAttackTargetCoroutine();
}