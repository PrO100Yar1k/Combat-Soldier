using System.Collections;
using UnityEngine;
using Zenject;
using System;

public abstract class BuildingController : MonoBehaviour, IDamagable, IDisposable
{
    [SerializeField] protected BuildingScriptable _buildingScriptable = default;

    [SerializeField] protected BuildingScreenCanvasController _buildingScreenCanvasController = default;
    [SerializeField] protected BuildingWorldCanvasController _buildingWorldCanvasController = default;

    public UICanvasController<BuildingController> UIController { get; protected set; }
    public HPControllerBuilding HPController { get; protected set; }

    public BuildingScriptable BuildingScriptable => _buildingScriptable;

    protected BaseBuildingAttack _buildingAttack = default;

    protected GameEventBus _gameEvents = default;
    protected RepositoryManager _repositoryManager = default;

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
    public void Construct(GameEventBus gameEvents, RepositoryManager repositoryManager)
    {
        _gameEvents = gameEvents;
        _repositoryManager = repositoryManager;
    }

    public virtual void InitializeBuilding()
    {
        UIController = new UICanvasController<BuildingController>(this, _buildingScreenCanvasController, _buildingWorldCanvasController, _gameEvents);
        HPController = new HPControllerBuilding(this, _buildingScreenCanvasController, _buildingScriptable);

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