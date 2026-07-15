using System.Collections.Generic;
using Assets.App.Scripts;
using System;

public class PlayerStateController : TroopStateController
{
    public PlayerStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ITroopAnimator animationController)
    {
        _states = new Dictionary<Type, TroopBaseState>
        {
            { typeof(TroopDefaultState), new PlayerDefaultState(repositoryManager, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopDefenseState), new PlayerDefenseState(repositoryManager, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopAttackState),  new PlayerAttackState(repositoryManager, troopController, screenCanvasController, this, animationController) },
            { typeof(TroopMoveState),    new PlayerMoveState(repositoryManager, troopController, screenCanvasController, this, animationController) },

            { typeof(TroopDeathState),   new PlayerDeathState(repositoryManager, troopController, screenCanvasController, this, animationController) }
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
