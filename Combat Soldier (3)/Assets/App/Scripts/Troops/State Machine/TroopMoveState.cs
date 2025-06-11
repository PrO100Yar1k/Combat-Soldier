using UnityEngine;
using DG.Tweening;
using System;

public class TroopMoveState : TroopBaseState
{
    private event Action<Vector3, Action> OnActivateTroopMovement = default;

    private Tween _movementTweenerController = default;
    private Tween _rotationTweenerController = default;

    #region Events

    private void SubscribeToEvents()
    {
        OnActivateTroopMovement += SetWaypoint;
    }

    private void UnSubscribeFromEvents()
    {
        OnActivateTroopMovement -= SetWaypoint;
    }

    #endregion

    public TroopMoveState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        EnableStateIcon();
        SubscribeToEvents();

        GameEvents.instance.TroopStartedMovement(); // for vision controller - maybe change
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();

        GameEvents.instance.TroopFinishedMovement(); // for vision controller - maybe change
    }

    public void ActivateTroopMovement(Vector3 point, Action finishAction)
        => OnActivateTroopMovement?.Invoke(point, finishAction);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/movement_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void SetWaypoint(Vector3 point, Action finishAction)
    {
        Transform troopTransform = _troopController.transform;

        Vector3 currentPos = troopTransform.position;
        Vector3 pointPos = new Vector3(point.x, currentPos.y, point.z);

        Vector3 offset = (pointPos - currentPos).normalized * 0.1f;
        Vector3 finalPos = new Vector3(pointPos.x - offset.x, currentPos.y, pointPos.z - offset.z);

        SmoothlyRotateTroop(finalPos.normalized);

        float distance = Vector3.Distance(finalPos, currentPos);

        float timeToArrive = distance / _troopScriptable.Speed;

        _movementTweenerController?.Kill();
        _movementTweenerController = troopTransform.DOMove(finalPos, timeToArrive)
            .SetEase(Ease.Flash)
            .OnComplete(delegate { Finished(finishAction); });
    }

    private void SmoothlyRotateTroop(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        const float rotationSpeed = 180f;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Quaternion.Angle(_troopController.transform.rotation, targetRotation);

        float rotationDuration = angle / rotationSpeed;

        _rotationTweenerController?.Kill();
        _rotationTweenerController = _troopController.transform.DORotate(new Vector3(0, targetAngle, 0), rotationDuration) // normalization // + targetAngle / 6
            .SetEase(Ease.Flash);
    }

    private void Finished(Action finishAction) // make like a target state after finishing point instead of event ?
    {
        Debug.Log("Finished Waypoint!");

        _switcherState.SwitchState<TroopDefaultState>(); // ??

        finishAction?.Invoke(); // change state to another
    }
}
