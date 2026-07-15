using System.Collections.Generic;
using Assets.App.Scripts;
using UnityEngine;
using System;

public abstract class TroopStateController : ISwitchableState, IDisposable
{
    protected ITroopAnimator _troopAnimationController = default;

    protected Dictionary<Type, TroopBaseState> _states = new();
    protected TroopBaseState _currentState;

    #region Disposable

    public void Dispose()
    {
        foreach (var state in _states.Values)
            state.Dispose();

        _states.Clear();
    }

    #endregion

    public TGet GetState<TGet>() where TGet : TroopBaseState
    {
        if (_states.TryGetValue(typeof(TGet), out var state))
            return (TGet) state;

        Debug.LogError($"[StateController] State {typeof(TGet).Name} is not registered!");
        return null;
    }

    public void NotifyActiveStateForTakingDamage<TState>(TState target) where TState : MonoBehaviour, IDamagable
    {
        (_currentState as IReactableForDamage)?.ReactionForTakingDamage(target); // future feature
    }

    public void ActivateAttackState(IDamagable enemyDamagable)
    {
        if (!CheckStateForActivity<TroopAttackState>())
            SwitchState<TroopAttackState>();

        GetState<TroopAttackState>()?.ActivateAttack(enemyDamagable);
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        if (!CheckStateForActivity<TroopDefenseState>())
            SwitchState<TroopDefenseState>();

        GetState<TroopDefenseState>()?.ActivateDefenseUnderAttack(enemyDamagable, enemyPosition);
    }

    public void ActivateMoveState(Vector3 targetPoint)
    {
        SwitchState<TroopMoveState>();
        GetState<TroopMoveState>()?.ActivateTroopMovement(targetPoint);
    }

    public void ActivateDefaultState()
    {
        if (CheckStateForActivity<TroopDefaultState>())
            Debug.LogError("Changed Default State for a 2 time in a row!");

        SwitchState<TroopDefaultState>();
    }

    public void ActivateDeathState()
    {
        SwitchState<TroopDeathState>();
    }

    public bool CheckStateForActivity<TCheck>() where TCheck : TroopBaseState
    {
        return _currentState is TCheck;
    }

    public void SwitchState<TSwitch>() where TSwitch : TroopBaseState
    {
        Type targetType = typeof(TSwitch);

        if (!_states.TryGetValue(targetType, out var nextState))
        {
            Debug.LogError($"[StateController] State {typeof(TSwitch).Name} was not found!");
            return;
        }

        _currentState?.Stop();
        _currentState = nextState;
        _currentState.Start();
    }
}

public interface ISwitchableState
{
    public void SwitchState<T>() where T : TroopBaseState;
}
