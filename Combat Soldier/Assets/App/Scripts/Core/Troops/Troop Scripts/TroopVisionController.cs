using UnityEngine;

public class TroopVisionController
{
    private readonly TroopController _troopController;
    private readonly TroopScriptable _troopScriptable;
    private readonly RepositoryManager _repositoryManager;

    public TroopVisionController(TroopController troopController, TroopScriptable troopScriptable, RepositoryManager repositoryManager)
    {
        _troopController = troopController;
        _troopScriptable = troopScriptable;

        _repositoryManager = repositoryManager;
    }

    public TroopController[] GetEnemiesInVisionRange()
    {
        Faction enemyTroopSide = GetEnemyTroopSide();

        float viewRange = _troopScriptable.ViewRangeRadius;
        Vector3 currentPosition = _troopController.transform.position;

        return _repositoryManager.GetEnemyListInRange(currentPosition, viewRange, enemyTroopSide);
    }

    private Faction GetEnemyTroopSide()
    {
        return _troopScriptable.TroopSide == Faction.Allies ? Faction.Enemies : Faction.Allies;
    }
}
