using UnityEngine;

public class WorldCanvasController : CanvasController
{
    [SerializeField] private RectTransform _attackCircleRange = default;
    [SerializeField] private RectTransform _viewCircleRange = default;

    public override void InitializeCanvas(TroopController troopController)
    {
        _troopController = troopController;
        _troopScriptable = troopController.TroopScriptable;

        SetupCircles();
    }

    private void SetupCircles()
    {
        ChangeCirclesState(false);
        SetupCircleRanges();
    }

    public override void EnableCanvas()
    {
        ChangeCirclesState(true);

        _canvasComponent.enabled = true;
    }

    public override void DisableCanvas()
    {
        ChangeCirclesState(false);

        _canvasComponent.enabled = false;
    }

    public void SetupCircleRanges()
    {
        float attackRangeRadius = _troopScriptable.attackRangeRadius;
        float viewRangeRadius = _troopScriptable.viewRangeRadius;

        _attackCircleRange.sizeDelta = new Vector2(attackRangeRadius, attackRangeRadius);
        _viewCircleRange.sizeDelta = new Vector2(viewRangeRadius, viewRangeRadius);
    }

    public void ChangeCirclesState(bool state)
    {
        _attackCircleRange.gameObject.SetActive(state);
        _viewCircleRange.gameObject.SetActive(state);
    }
}
