using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TroopStateController : ISwitchableState
{
    private readonly TroopController _troopController = default;

    private TroopDefaultState _troopDefaultState = default;
    private TroopMoveState _troopMoveState = default;
    private TroopAttackState _troopAttackState = default;
    private TroopDefenseState _troopDefenseState = default;

    public TroopDefenseState TroopDefenseState => _troopDefenseState; //

    private List<TroopBaseState> _allStates = default;
    private TroopBaseState _currentState = default;

    public TroopStateController(TroopController troopController)
    {
        _troopController = troopController;

        InitializeBaseTroopStates();
    }

    private void InitializeBaseTroopStates()
    {
        _troopDefaultState = new TroopDefaultState(_troopController, this);
        _troopMoveState = new TroopMoveState(_troopController, this);
        _troopAttackState = new TroopAttackState(_troopController, this);
        _troopDefenseState = new TroopDefenseState(_troopController, this);

        _allStates = new List<TroopBaseState>() { _troopDefaultState, _troopMoveState, _troopAttackState, _troopDefenseState };

        _currentState = _allStates[0];
    }


    public bool CheckStateForActivity<State>() where State : TroopBaseState
    {
        TroopBaseState state = _allStates.FirstOrDefault(s => s is State);

        return _currentState == state;
    }

    public void ActivateDefaultState()
    {
        SwitchState<TroopDefaultState>();
    }
    
    public void ActivateAttackState(TroopController enemy)
    {
        SwitchState<TroopAttackState>();

        _troopAttackState.ActivateTroopAttack(enemy);
    }

    public void ActivateDefenceState()
    {
        SwitchState<TroopDefenseState>();
    }

    public void ActivateMoveState(Vector3 targetPoint, Action finishAction)
    {
        SwitchState<TroopMoveState>();

        _troopMoveState.ActivateTroopMovement(targetPoint, finishAction);
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
