using System.Collections.Generic;

public class PlayerTroopStateController : TroopStateController
{
    public PlayerTroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController) : base(troopController, screenCanvasController)
    {
        _troopDefaultState = new PlayerTroopDefaultState(_troopController, screenCanvasController, this);
        _troopDefenseState = new PlayerTroopDefenseState(_troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        ActivateDefaultState();
    }
}
