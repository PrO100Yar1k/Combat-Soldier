using UnityEngine;

public abstract class TroopController : MonoBehaviour
{
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [Space(2)]

    [SerializeField] protected TroopCanvasController _canvasController = default;

    [Space(2)]

    [SerializeField] protected TroopSide _troopSide = default;

    public TroopStateController StateController { get; protected set; }

    public TroopVisionController VisionController { get; protected set; } 
    
    public TroopUIController UIController { get; protected set; }

    public HPController HPController { get; protected set; }

    public TroopScriptable TroopScriptable => _troopScriptable; // ?

    protected virtual void OnEnable() 
        => GameEvents.instance.TroopSpawned(this, _troopSide);

    protected virtual void OnDisable()
        => GameEvents.instance.TroopDied(this, _troopSide);

    protected virtual void Awake()
        => InitializeTroop();


    protected abstract void InitializeTroop();
}

public enum TroopSide
{
    Player,
    Enemy
}