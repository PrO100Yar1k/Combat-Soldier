using UnityEngine;

public class TroopController : MonoBehaviour
{
    [SerializeField] private TroopScriptable _troopScriptable = default;

    public TroopStateController StateController { get; private set; }
    public HPController HPController { get; private set; }
    public TroopUIController UIController { get; private set; }

    private void Awake()
    {
        StateController = new TroopStateController(this);
        HPController = new HPController(_troopScriptable);

        UIController = new TroopUIController(); // to do
    }
}