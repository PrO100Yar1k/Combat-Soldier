using Assets.App.Scripts.Core.Canvases;
using Assets.App.Scripts;
using UnityEngine;
using System;
using Pathfinding;
using System.Collections;

public abstract class TroopMoveState : TroopBaseState
{
    private event Action<Vector3> OnActivateTroopMovement = default;

    private readonly IAstarAI _ai;
    private Coroutine _checkArrivalCoroutine;

    protected override string StateIconLocation
        => "State Icons/Move-State-Icon";

    protected TroopMoveState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {
        _ai = _troopController.GetComponent<IAstarAI>();
    }

    #region Events

    protected override void SubscribeToEvents()
    {
        OnActivateTroopMovement += SetWaypoint;
    }

    protected override void UnSubscribeFromEvents()
    {
        OnActivateTroopMovement -= SetWaypoint;
    }

    #endregion

    public override void OnStart()
    {
        PlayStateAnimation();

        if (_ai != null)
        {
            _ai.maxSpeed = _troopScriptable.Speed; // Встановлюємо швидкість із ScriptableObject
            _ai.isStopped = false; // Дозволяємо рух при вході в стан
        }
    }

    public override void OnStop()
    {
        StopMovement();
        StopCheckArrivalCoroutine();
    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayRunning();
    }

    public void ActivateTroopMovement(Vector3 point)
    {
        OnActivateTroopMovement?.Invoke(point);
    }

    private void SetWaypoint(Vector3 point)
    {
        if (_ai == null)
            return;

        _ai.destination = point;
        _ai.isStopped = false;
        _ai.SearchPath();

        StopCheckArrivalCoroutine();
        _checkArrivalCoroutine = _troopController.StartCoroutine(WaitUntilReachedDestination());
    }
    private IEnumerator WaitUntilReachedDestination()
    {
        yield return null;

        while (_ai.pathPending || !_ai.reachedDestination)
        {
            yield return new WaitForSeconds(0.1f); // Перевіряємо раз на 100мс (економить ресурси)
        }

        ActionAfterFinish();
    }

    private void StopCheckArrivalCoroutine()
    {
        if (_checkArrivalCoroutine != null)
        {
            _troopController.StopCoroutine(_checkArrivalCoroutine);
            _checkArrivalCoroutine = null;
        }
    }

    private void StopMovement()
    {
        if (_ai != null)
        {
            _ai.isStopped = true;
        }
    }

    private void ActionAfterFinish()
    {
        _switcherState.SwitchState<TroopDefaultState>();
    }
}
