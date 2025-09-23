using UnityEngine;
using System;

public abstract class TroopController : MonoBehaviour, IDisposable, IDamagable, IResistable
{
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [SerializeField] protected TroopScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected WorldCanvasController _worldCanvasController = default;

    public UICanvasController<TroopController> UIController { get; protected set; } //
    public TroopStateController StateController { get; protected set; } //
    public HPControllerTroop HPController { get; protected set; } //

    public TroopScriptable TroopScriptable => _troopScriptable;
    protected TroopSide _troopSide => _troopScriptable.TroopSide;

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
        => HPController.TakeDamage(attackDamage);

    public void ActivateDefenseUnderAttack(IDamagable enemyIDamagable, Vector3 enemyPosition)
    {
        if (StateController.CheckStateForActivity<TroopDefaultState>())
            StateController.ActivateDefenceState();

        StateController.ActivateDefenseUnderAttack(enemyIDamagable, enemyPosition);
    }

    #endregion

    protected abstract void InitializeTroop();
}

public enum TroopSide
{
    Player,
    Enemy
}