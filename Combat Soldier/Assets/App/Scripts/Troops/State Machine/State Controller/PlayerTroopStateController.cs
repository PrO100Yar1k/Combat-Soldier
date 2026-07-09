using System.Collections.Generic;

public class PlayerTroopStateController : TroopStateController
{
    public PlayerTroopStateController(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController)
    {
        _troopDefaultState = new PlayerDefaultState(repositoryManager, troopController, screenCanvasController, this);
        _troopDefenseState = new PlayerDefenseState(repositoryManager, troopController, screenCanvasController, this);
        _troopAttackState = new PlayerAttackState(repositoryManager, troopController, screenCanvasController, this);
        _troopMoveState = new PlayerMoveState(repositoryManager, troopController, screenCanvasController, this);
        _troopDeathState = new PlayerDeathState(repositoryManager, troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        SwitchState<TroopDefaultState>();
    }
}
