using UnityEngine;
using System;

public abstract class TroopController : MonoBehaviour, IDisposable, IDamagable, IReactableForDamage
{
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [SerializeField] protected TroopScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected WorldCanvasController _worldCanvasController = default;

    public UICanvasController<TroopController> UIController { get; protected set; } //
    public TroopStateController StateController { get; protected set; } //
    public HPControllerTroop HPController { get; protected set; } //

    public TroopScriptable TroopScriptable => _troopScriptable;

    protected TroopSide _troopSide => _troopScriptable.TroopSide;

    protected event Action OnNotificationForGettingDamaged = default;

    #region Events & Interface Implemention

    protected virtual void OnEnable() 
        => GameEvents.instance.TroopSpawned(this, _troopSide);

    protected virtual void OnDisable()
        => GameEvents.instance.TroopDied(this, _troopSide);

    protected virtual void Awake()
        => InitializeTroop();

    public void Dispose()
    {
        UIController.Dispose();
        StateController.Dispose();
    }

    public void TakeDamage(int attackDamage)
    {
        HPController.TakeDamage(attackDamage);
        OnNotificationForGettingDamaged?.Invoke();
    }

    #endregion

    protected abstract void InitializeTroop();

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        if (StateController.CheckStateForActivity<TroopAttackState>())
            return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.transform.position;

        float attackRange = _troopScriptable.AttackRangeRadius;

        if (Vector3.Distance(currentPos, targetPos) > attackRange)
            return;

        StateController.ActivateDefenseUnderAttack(target, targetPos);
    }


    protected TroopSide GetEnemyTroopSide()
        => _troopSide == TroopSide.Player ? TroopSide.Enemy : TroopSide.Player;
}

public enum TroopSide
{
    Player,
    Enemy
}