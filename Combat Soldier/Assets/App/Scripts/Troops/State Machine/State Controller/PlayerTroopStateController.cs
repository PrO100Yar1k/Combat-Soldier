using System.Collections.Generic;

public class PlayerTroopStateController : TroopStateController
{
    public PlayerTroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController) : base(troopController, screenCanvasController)
    {
        _troopDefaultState = new PlayerDefaultState(troopController, screenCanvasController, this);
        _troopDefenseState = new PlayerDefenseState(troopController, screenCanvasController, this);
        _troopAttackState = new PlayerAttackState(troopController, screenCanvasController, this);
        _troopMoveState = new PlayerMoveState(troopController, screenCanvasController, this);
        _troopDeathState = new PlayerDeathState(troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        //ActivateDefaultState();
        SwitchState<TroopDefaultState>();
    }
}
