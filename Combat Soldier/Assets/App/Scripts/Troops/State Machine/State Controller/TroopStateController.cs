using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class TroopStateController : ISwitchableState, IDisposable
{
    protected readonly TroopController _troopController = default;

    protected TroopDefaultState _troopDefaultState = default;
    protected TroopMoveState _troopMoveState = default;
    protected TroopAttackState _troopAttackState = default;
    protected TroopDefenseState _troopDefenseState = default;
    protected TroopDeathState _troopDeathState = default;

    protected List<TroopBaseState> _allStates = default;

    protected TroopBaseState _currentState = default;

    public TroopStateController(TroopController troopController, TroopScreenCanvasController screenCanvasController) 
    {
        _troopController = troopController;

        _troopMoveState = new TroopMoveState(_troopController, screenCanvasController, this);
        _troopAttackState = new TroopAttackState(_troopController, screenCanvasController, this);
        _troopDeathState = new TroopDeathState(_troopController, screenCanvasController, this);

    }

    public void Dispose()
    {
        foreach (IDisposable disposableState in _allStates)
            disposableState.Dispose();
    }

    public void NotifyActiveStateForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        (_currentState as IReactableForDamage)?.ReactionForTakingDamage(target);
    }

    public void ActivateDefaultState()
    {
        SwitchState<TroopDefaultState>();
    }
    
    public void ActivateAttackState(IDamagable enemy)
    {
        SwitchState<TroopAttackState>();

        _troopAttackState.ActivateAttack(enemy);
    }

    public void ActivateDefenceState()
    {
        SwitchState<TroopDefenseState>();
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyIDamagable, Vector3 enemyPosition)
    {
        _troopDefenseState.ActivateDefenseUnderAttack(enemyIDamagable, enemyPosition);
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
