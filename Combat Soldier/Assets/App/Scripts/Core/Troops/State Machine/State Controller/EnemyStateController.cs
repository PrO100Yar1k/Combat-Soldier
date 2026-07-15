using UnityEngine;
using Assets.App.Scripts;
using System.Collections.Generic;

public class EnemyStateController : TroopStateController
{
    public EnemyStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList, ITroopAnimator animationController)
    {
        _troopDefaultState = new EnemyDefaultState(repositoryManager, troopController, screenCanvasController, this, targetPointsList, animationController); // targetPointsList to do
        _troopDefenseState = new EnemyDefenseState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopAttackState = new EnemyAttackState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopMoveState = new EnemyMoveState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopDeathState = new EnemyDeathState(repositoryManager, troopController, screenCanvasController, this, animationController);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };

        ActivateDefaultState();
    }
}
