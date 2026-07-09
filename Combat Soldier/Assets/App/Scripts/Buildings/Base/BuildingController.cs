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

    private GameEvents _gameEvents = default;

    #region Events & Interface Implemention

    protected virtual void OnEnable()
        => _gameEvents.BuildingSpawned(this);

    protected virtual void OnDisable()
        => _gameEvents.BuildingDestroyed(this);

    private void Awake() //
        => InitializeBuilding(); //

    public void Dispose()
        => UIController.Dispose();

    public void TakeDamage(int attackDamage)
        => HPController.TakeDamage(attackDamage);

    #endregion

    [Inject]
    public void Construct(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

    protected virtual void InitializeBuilding()
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
}

public interface IAttackable
{
    public void Attack(IDamagable attackTarget);
    public IEnumerator CheckAttackTargetCoroutine();
}