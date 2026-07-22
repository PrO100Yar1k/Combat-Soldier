using System.Collections.Generic;
using Assets.App.Scripts;
using UnityEngine;
using System;

public class EnemyStateController : TroopStateController
{
    public EnemyStateController(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList, ITroopAnimator animationController)
    {
        _states = new Dictionary<Type, TroopBaseState>
        {
            { typeof(TroopDefaultState), new EnemyDefaultState(targetSearchService, troopController, screenCanvasController, this, targetPointsList, animationController) },
            { typeof(TroopDefenseState), new EnemyDefenseState(targetSearchService, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopAttackState),  new EnemyAttackState(targetSearchService, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopMoveState),    new EnemyMoveState(targetSearchService, troopController, screenCanvasController, this, animationController) },

            { typeof(TroopDeathState),   new EnemyDeathState(targetSearchService, troopController, screenCanvasController, this, animationController) }
        };

        ActivateDefaultState();
    }
}