using UnityEngine;

public abstract class TroopController : MonoBehaviour
{
    [SerializeField] protected TroopScriptable _troopScriptable = default;

    [Space(2)]

    [SerializeField] protected ScreenCanvasController _screenCanvasController = default;
    [SerializeField] protected WorldCanvasController _worldCanvasController = default;

    [Space(2)]

    [SerializeField] protected TroopModelController _troopModelController = default;

    public TroopStateController StateController { get; protected set; }

    public TroopVisionController VisionController { get; protected set; } 
    
    public TroopUIController UIController { get; protected set; }

    public HPController HPController { get; protected set; }

    public TroopScriptable TroopScriptable => _troopScriptable; // ?

    protected TroopSide _troopSide => _troopScriptable.TroopSide;


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