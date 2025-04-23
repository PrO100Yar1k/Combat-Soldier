using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private TroopScriptable _troopScriptable = default;

    [Space(2)]

    [SerializeField] private TroopCanvasController _canvasController = default;

    [Space(2)]

    [SerializeField] private TroopSide _troopSide = default;

    public TroopStateController StateController { get; private set; } // ?

    public TroopVisionController VisionController { get; private set; } 
    
    public TroopUIController UIController { get; private set; }

    public HPController HPController { get; private set; }

    private void OnEnable() 
        => GameEvents.instance.TroopSpawned(this, _troopSide);

    private void OnDisable()
        => GameEvents.instance.TroopDied(this, _troopSide);

    private void Awake()
    {
        //to do setup correct sequence of scripts

        StateController = new TroopStateController(this);
        VisionController = new TroopVisionController(); // to do

        UIController = new TroopUIController(_troopScriptable, _canvasController);
        HPController = new HPController(_troopScriptable, _canvasController);

        HPController.TakeDamage(25); // test
    }
}

public enum TroopSide
{
    Player,
    Enemy
}