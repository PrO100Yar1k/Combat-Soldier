using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;
using System;
using UnityEngine;
using Zenject;

public abstract class TroopController : MonoBehaviour, IDisposable, IDamagable, IReactableForDamage, ICoroutineRunner
{
    [SerializeField] protected Transform _bulletInitialPoint = default;
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [SerializeField] protected BaseTroopModelController _troopModelController = default; //
    [SerializeField] protected TroopScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected TroopWorldCanvasController _worldCanvasController = default;

    [SerializeField] protected TroopAnimationController _animationController = default;

    public Transform BulletInitialPoint => _bulletInitialPoint;
    public BaseTroopModelController TroopModelController => _troopModelController;

    public UICanvasController<TroopController, TroopScriptable> UIController { get; protected set; }
    public TroopStateController StateController { get; protected set; }
    public HPTroopController HPController { get; protected set; }

    public TroopScriptable TroopScriptable => _troopScriptable;
    public Faction _troopSide => _troopScriptable.TroopSide;

    protected event Action OnNotificationForGettingDamaged = default;

    protected TargetSearchService _targetSearchService = default;
    protected GameEventBus _gameEventBus = default;

    #region Events & Interface Implemention

    protected virtual void OnEnable() 
        => _gameEventBus.TroopSpawned(this, _troopSide);

    protected virtual void OnDisable()
        => _gameEventBus.TroopDied(this, _troopSide);

    public void Dispose()
    {
        UIController.Dispose();
        StateController.Dispose();
    }

    public void TakeDamage(int attackDamage)
    {
        HPController.TakeDamage(attackDamage);
        OnNotificationForGettingDamaged?.Invoke();

        _worldCanvasController.StartTakingDamage();
    }

    public Faction GetFaction()
    {
        return _troopSide;
    }

    public void ChangeUnitCircleToReloading(float reloadingTime)
    {
        _worldCanvasController.StartReloading(reloadingTime);
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

public enum Faction
{
    None,
    Allies,
    Enemies
}