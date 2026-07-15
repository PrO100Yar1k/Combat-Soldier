using Assets.App.Scripts;
using UnityEngine;
using DG.Tweening;
using System;

public abstract class TroopMoveState : TroopBaseState
{
    private event Action<Vector3> OnActivateTroopMovement = default;

    private Tween _movementTweenerController = default;
    private Tween _rotationTweenerController = default;

    protected TroopMoveState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
    {

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
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void OnStop()
    {
        UnSubscribeFromEvents();
    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayRunning();
    }

    public void ActivateTroopMovement(Vector3 point)
    {
        OnActivateTroopMovement?.Invoke(point);
    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/movement_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void SetWaypoint(Vector3 point)
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
            .OnComplete(delegate { ActionAfterFinish(); });
    }

    private void SmoothlyRotateTroop(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero)
            return;

        const float rotationSpeed = 270f;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        float angle = Quaternion.Angle(_troopController.transform.rotation, targetRotation);
        float rotationDuration = angle / rotationSpeed;

        _rotationTweenerController?.Kill();
        _rotationTweenerController = _troopController.transform
            .DORotateQuaternion(targetRotation, rotationDuration)
            .SetEase(Ease.OutSine);
    }


    private void ActionAfterFinish()
    {
        _switcherState.SwitchState<TroopDefaultState>(); //
    }
}
