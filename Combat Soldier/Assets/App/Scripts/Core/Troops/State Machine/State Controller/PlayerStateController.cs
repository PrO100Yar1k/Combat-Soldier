using Assets.App.Scripts;
using System.Collections.Generic;

public class PlayerStateController : TroopStateController
{
    public PlayerStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ITroopAnimator animationController)
    {
        _troopDefaultState = new PlayerDefaultState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopDefenseState = new PlayerDefenseState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopAttackState = new PlayerAttackState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopMoveState = new PlayerMoveState(repositoryManager, troopController, screenCanvasController, this, animationController);
        _troopDeathState = new PlayerDeathState(repositoryManager, troopController, screenCanvasController, this, animationController);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };

        ActivateDefaultState();
    }

    public bool TrySwitchToOppositeState()
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
