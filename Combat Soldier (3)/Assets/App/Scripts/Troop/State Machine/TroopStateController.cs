using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TroopStateController : ISwitchableState
{
    private TroopController _troopController = default;

    private List<TroopBaseState> _allStates = default;
    private TroopBaseState _currentState = default;

    public TroopStateController(TroopController troopController)
    {
        _troopController = troopController;

        InitializeBaseTroopStates();
    }

    private void InitializeBaseTroopStates()
    {
        _allStates = new List<TroopBaseState>()
        {
            new TroopDefaultState(_troopController, this),
            new TroopMoveState(_troopController, this),
            new TroopAttackState(_troopController, this),
            new TroopDefenseState(_troopController, this)
        };

        _currentState = _allStates[0];
    }

    public void ActivateDefaultState()
    {
        SwitchState<TroopDefaultState>();
    }
    
    public void ActivateAttackState()
    {
        SwitchState<TroopAttackState>();
    }

    public void ActivateDefenceState()
    {
        SwitchState<TroopDefenseState>();
    }

    public void ActivateMoveState()
    {
        SwitchState<TroopMoveState>();
    }

    public void SwitchState<T>() where T : TroopBaseState
    {
        TroopBaseState state = _allStates.FirstOrDefault(s => s is T);

        _currentState.Stop();
        _currentState = state;

        _currentState.Start();
    }
}

public interface ISwitchableState
{
    public void SwitchState<T>() where T : TroopBaseState;
}
