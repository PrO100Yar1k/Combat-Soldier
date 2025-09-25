using UnityEngine;

public class TrenchUnit : MonoBehaviour
{
    private TroopController _troopInsideTrench = default;

    public void EntryTroopInsideTrench(TroopController troopController)
    {
        if (_troopInsideTrench != null)
            return;

        if (troopController == null)
            return;

        _troopInsideTrench = troopController;
    }

    public void ExitTroopFromTrench(TroopController troopController)
    {
        if (troopController != _troopInsideTrench)
            return;

        _troopInsideTrench = null;
    }
}
