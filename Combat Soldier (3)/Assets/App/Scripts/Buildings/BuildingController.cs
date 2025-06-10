using UnityEngine;

public class BuildingController : MonoBehaviour, IDamagable
{
    [SerializeField] private BuildingScriptable _buildingScriptable = default;

    [SerializeField] private TroopController _troopInsideBuilding = default;

    private void Start()
    {
        // to do
    }

    public void TakeDamage(int attackDamage)
    {

    }

    public void GetTroopInsideBuilding(TroopController troopController)
    {
        if (_troopInsideBuilding != null)
            return;

        _troopInsideBuilding = troopController;
    }
}

public interface IDamagable
{
    public void TakeDamage(int attackDamage);
}