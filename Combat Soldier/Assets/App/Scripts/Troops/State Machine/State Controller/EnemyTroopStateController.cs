using System.Collections.Generic;
using UnityEngine;

public class EnemyTroopStateController : TroopStateController
{
    public EnemyTroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList) : base(troopController, screenCanvasController)
    {
        _troopDefaultState = new EnemyDefaultState(troopController, screenCanvasController, this, targetPointsList);
        _troopDefenseState = new EnemyDefenseState(troopController, screenCanvasController, this);
        _troopAttackState = new EnemyAttackState(troopController, screenCanvasController, this);
        _troopMoveState = new EnemyMoveState(troopController, screenCanvasController, this);
        _troopDeathState = new EnemyDeathState(troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        //ActivateDefaultState();
        SwitchState<TroopDefaultState>();
    }
}
