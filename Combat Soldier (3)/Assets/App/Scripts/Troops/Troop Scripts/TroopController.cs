using UnityEngine;

public abstract class TroopController : MonoBehaviour, IDamagable
{
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [Space(2)]

    [SerializeField] protected ScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected WorldCanvasController _worldCanvasController = default;

    public TroopStateController StateController { get; protected set; }    
    public TroopUIController UIController { get; protected set; }
    public HPController HPController { get; protected set; }

    public TroopScriptable TroopScriptable => _troopScriptable;

    protected TroopSide _troopSide => _troopScriptable.TroopSide;

    protected abstract void InitializeTroop();

    protected virtual void OnEnable() 
        => GameEvents.instance.TroopSpawned(this, _troopSide);

    protected virtual void OnDisable()
        => GameEvents.instance.TroopDied(this, _troopSide);

    protected virtual void Awake()
        => InitializeTroop();

    public void TakeDamage(int attackDamage)
        => HPController.TakeDamage(attackDamage);
}

public enum TroopSide
{
    Player,
    Enemy
}