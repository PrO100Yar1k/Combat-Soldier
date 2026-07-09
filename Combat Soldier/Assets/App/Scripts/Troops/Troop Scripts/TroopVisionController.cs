using UnityEngine;
using Zenject;

public class TroopVisionController
{
    private TroopController _troopController = default;
    private TroopScriptable _troopScriptable = default;

    private RepositoryManager _repositoryManager = default;


    public bool IsTroopVisible { get; private set; } = true;

    public TroopVisionController(TroopController troopController, TroopScriptable troopScriptable)
    {
        _troopController = troopController;
        _troopScriptable = troopScriptable;
    }

    [Inject]
    public void Construct(RepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public TroopController[] GetEnemiesInVisionRange()
    {
        Vector3 currentPosition = _troopController.transform.position;
        float viewRange = _troopScriptable.ViewRangeRadius;

        TroopSide enemyTroopSide = GetEnemyTroopSide();

        return _repositoryManager.GetEnemyListInRange(currentPosition, viewRange, enemyTroopSide);
    }

    private TroopSide GetEnemyTroopSide()
        => _troopScriptable.TroopSide == TroopSide.Player ? TroopSide.Enemy : TroopSide.Player;
}
