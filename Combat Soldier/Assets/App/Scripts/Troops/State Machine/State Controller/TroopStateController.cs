using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class TroopStateController : ISwitchableState, IDisposable
{
    protected TroopDefaultState _troopDefaultState = default;
    protected TroopMoveState _troopMoveState = default;
    protected TroopAttackState _troopAttackState = default;
    protected TroopDefenseState _troopDefenseState = default;
    protected TroopDeathState _troopDeathState = default;

    protected List<TroopBaseState> _allStates = default;

    protected TroopBaseState _currentState = default;

    public TroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController) { }

    public void Dispose()
    {
        foreach (IDisposable disposableState in _allStates)
            disposableState.Dispose();
    }

    public void NotifyActiveStateForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        (_currentState as IReactableForDamage)?.ReactionForTakingDamage(target); // future feature
    }

    public void ActivateAttackState(IDamagable enemyDamagable)
    {
        if (CheckStateForActivity<TroopAttackState>() == false)
            SwitchState<TroopAttackState>();

        _troopAttackState.ActivateAttack(enemyDamagable);
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyDamagable, Vector3 enemyPosition) // activate just defense state
    {
        if (CheckStateForActivity<TroopDefenseState>() == false)
            SwitchState<TroopDefenseState>();

        _troopDefenseState.ActivateDefenseUnderAttack(enemyDamagable, enemyPosition);
    }

    public void ActivateMoveState(Vector3 targetPoint)
    {
        SwitchState<TroopMoveState>();

        _troopMoveState.ActivateTroopMovement(targetPoint);
    }

    public void ActivateDefaultState()
    {
        if (CheckStateForActivity<TroopDefaultState>() == false)
            SwitchState<TroopDefaultState>();
    }

    public void ActivateDeathState()
    {
        //if (!CheckStateForActivity<TroopDeathState>())
        SwitchState<TroopDeathState>();
    }

    public bool CheckStateForActivity<State>() where State : TroopBaseState
    {
        return _currentState == _allStates.FirstOrDefault(s => s is State);
    }

    public void SwitchState<State>() where State : TroopBaseState
    {
        TroopBaseState state = _allStates.FirstOrDefault(s => s is State);

        _currentState.Stop();
        _currentState = state;
        _currentState.Start();

        Debug.Log(_currentState);
    }
}

public interface ISwitchableState
{
    public void SwitchState<T>() where T : TroopBaseState;
}
