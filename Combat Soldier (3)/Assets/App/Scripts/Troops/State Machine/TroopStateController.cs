using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TroopStateController : ISwitchableState
{
    private readonly TroopController _troopController = default;

    private readonly TroopDefaultState _troopDefaultState = default;
    private readonly TroopMoveState _troopMoveState = default;
    private readonly TroopAttackState _troopAttackState = default;
    private readonly TroopDefenseState _troopDefenseState = default;
    private readonly TroopDeathState _troopDeathState = default;

    private readonly List<TroopBaseState> _allStates = default;

    private TroopBaseState _currentState = default;

    public TroopStateController(TroopController troopController, ScreenCanvasController screenCanvasController)
    {
        _troopController = troopController;

        _troopDefaultState = new TroopDefaultState(_troopController, screenCanvasController, this);
        _troopMoveState = new TroopMoveState(_troopController, screenCanvasController, this);
        _troopAttackState = new TroopAttackState(_troopController, screenCanvasController, this);
        _troopDefenseState = new TroopDefenseState(_troopController, screenCanvasController, this);
        _troopDeathState = new TroopDeathState(_troopController, screenCanvasController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState, _troopDeathState };
        _currentState = _allStates[0];

        ActivateDefaultState();
    }

    public void ActivateDefaultState()
    {
        SwitchState<TroopDefaultState>();
    }
    
    public void ActivateAttackState(IDamagable enemy)
    {
        SwitchState<TroopAttackState>();

        _troopAttackState.ActivateTroopAttack(enemy);
    }

    public void ActivateDefenceState()
    {
        SwitchState<TroopDefenseState>();
    }

    public void ActivateDefenseUnderAttack(HPController enemyHPController)
    {
        _troopDefenseState.ActivateDefenseUnderAttack(enemyHPController);
    }

    public void ActivateMoveState(Vector3 targetPoint, Action finishAction)
    {
        SwitchState<TroopMoveState>();

        _troopMoveState.ActivateTroopMovement(targetPoint, finishAction);
    }
    
    public void ActivateDeathState()
    {
        SwitchState<TroopDeathState>();
    }

    public bool CheckStateForActivity<State>() where State : TroopBaseState
    {
        TroopBaseState state = _allStates.FirstOrDefault(s => s is State);

        return _currentState == state;
    }

    public void SwitchState<State>() where State : TroopBaseState
    {
        TroopBaseState state = _allStates.FirstOrDefault(s => s is State);

        _currentState.Stop();
        _currentState = state;
        _currentState.Start();
    }
}

public interface ISwitchableState
{
    public void SwitchState<T>() where T : TroopBaseState;
}
