using System.Collections.Generic;

public class EnemyTroopStateController : TroopStateController
{
    public EnemyTroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController) : base(troopController, screenCanvasController)
    {
        _troopDefaultState = new EnemyDefaultState(_troopController, screenCanvasController, this);
        _troopDefenseState = new EnemyDefenseState(_troopController, screenCanvasController, this);
        _troopAttackState = new EnemyAttackState(_troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        SwitchState<TroopDefaultState>();
    }
}
