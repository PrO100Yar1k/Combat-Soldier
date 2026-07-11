using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : TroopStateController
{
    public EnemyStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList)
    {
        _troopDefaultState = new EnemyDefaultState(repositoryManager, troopController, screenCanvasController, this, targetPointsList);
        _troopDefenseState = new EnemyDefenseState(repositoryManager, troopController, screenCanvasController, this);
        _troopAttackState = new EnemyAttackState(repositoryManager, troopController, screenCanvasController, this);
        _troopMoveState = new EnemyMoveState(repositoryManager, troopController, screenCanvasController, this);
        _troopDeathState = new EnemyDeathState(repositoryManager, troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };

        ActivateDefaultState();
    }
}
