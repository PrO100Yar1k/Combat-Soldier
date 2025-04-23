using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private TroopScriptable _troopScriptable = default;

    [Space(2)]

    [SerializeField] private TroopCanvasController _troopCanvasController = default;

    public TroopStateController StateController { get; private set; }

    public HPController HPController { get; private set; }

    public TroopUIController UIController { get; private set; }

    private void Awake()
    {
        //to do setup correct sequence of scripts

        StateController = new TroopStateController(this);
        HPController = new HPController(_troopScriptable, _troopCanvasController);

        UIController = new TroopUIController(); // to do

        _troopCanvasController.InitializeScriptableObject(_troopScriptable);

        //test
        HPController.TakeDamage(25);
    }
}