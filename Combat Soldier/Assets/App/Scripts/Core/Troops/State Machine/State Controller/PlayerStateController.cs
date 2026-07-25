using Assets.App.Scripts.Core.Canvases;
using System.Collections.Generic;
using Assets.App.Scripts;
using System;

public class PlayerStateController : TroopStateController
{
    public PlayerStateController(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ITroopAnimator animationController)
    {
        _states = new Dictionary<Type, TroopBaseState>
        {
            { typeof(TroopDefaultState), new PlayerDefaultState(targetSearchService, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopDefenseState), new PlayerDefenseState(targetSearchService, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopAttackState),  new PlayerAttackState(targetSearchService, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopMoveState),    new PlayerMoveState(targetSearchService, troopController, screenCanvasController, this, animationController) },

            { typeof(TroopDeathState),   new PlayerDeathState(targetSearchService, troopController, screenCanvasController, this, animationController) }
        };

        ActivateDefaultState();
    }

    public bool TrySwitchToOppositeState() //maybe remove this feature
    {
        bool isAttack = CheckStateForActivity<TroopAttackState>();
        bool isDefense = CheckStateForActivity<TroopDefenseState>();

        if (!isAttack && !isDefense)
            return false;

        if (isAttack) SwitchState<TroopDefenseState>();
        else if (isDefense) SwitchState<TroopAttackState>();

        return true;
    }
}
