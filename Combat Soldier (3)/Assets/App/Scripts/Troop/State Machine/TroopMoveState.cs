using UnityEngine;
using DG.Tweening;

public class TroopMoveState : TroopBaseState
{
    public TroopMoveState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState) { }

    private bool _isRunning = false; // to do

    private Tween _movementTween;
    private Tween _rotationTween;

    #region Events

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopMovement += SetWaypoint;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopMovement -= SetWaypoint;
    }

    #endregion

    public override void Start()
    {
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    private void SetWaypoint(Vector3 point, float speed)
    {
        _isRunning = true;

        Transform troopTransform = _troopController.transform;

        Vector3 currentPos = troopTransform.position;
        Vector3 pointPos = new Vector3(point.x, currentPos.y, point.z);

        Vector3 offset = (pointPos - currentPos).normalized * 0.1f;
        Vector3 finalPos = new Vector3(pointPos.x - offset.x, currentPos.y, pointPos.z - offset.z);

        SmoothlyRotateTroop(finalPos.normalized);

        float distance = Vector3.Distance(finalPos, currentPos);
        float timeToArrive = distance / speed;

        _movementTween?.Kill();
        _movementTween = troopTransform.DOMove(finalPos, timeToArrive)
            .SetEase(Ease.Flash)
            .OnComplete(Finished);
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

        _rotationTween?.Kill();
        _rotationTween = _troopController.transform.DORotate(new Vector3(0, targetAngle, 0), rotationDuration) // normalization // + targetAngle / 6
            .SetEase(Ease.Flash);
    }

    private void Finished()
    {
        _isRunning = false;

        Debug.Log("Finished Waypoint!");

        _switcherState.SwitchState<TroopDefaultState>();
    }
}
