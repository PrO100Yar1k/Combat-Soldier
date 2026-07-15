using System.Collections.Generic;
using Assets.App.Scripts;
using UnityEngine;
using System;

public class EnemyStateController : TroopStateController
{
    public EnemyStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList, ITroopAnimator animationController)
    {
        _states = new Dictionary<Type, TroopBaseState>
        {
            { typeof(TroopDefaultState), new EnemyDefaultState(repositoryManager, troopController, screenCanvasController, this, targetPointsList, animationController) },
            { typeof(TroopDefenseState), new EnemyDefenseState(repositoryManager, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopAttackState),  new EnemyAttackState(repositoryManager, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopMoveState),    new EnemyMoveState(repositoryManager, troopController, screenCanvasController, this, animationController) },

            { typeof(TroopDeathState),   new EnemyDeathState(repositoryManager, troopController, screenCanvasController, this, animationController) }
        };

        ActivateDefaultState();
    }
}