using UnityEngine;

public abstract class WorldCanvasController : TroopCanvasController
{
    [SerializeField] private RectTransform _attackCircleRange = default;
    [SerializeField] private RectTransform _viewCircleRange = default;

    protected override void AssignDefaultCanvasValues()
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
        float attackRangeRadius = _troopScriptable.AttackRangeRadius;
        float viewRangeRadius = _troopScriptable.ViewRangeRadius;

        _attackCircleRange.localScale = new Vector2(attackRangeRadius * 2, attackRangeRadius * 2);
        _viewCircleRange.localScale = new Vector2(viewRangeRadius * 2, viewRangeRadius * 2);
    }

    public void ChangeCirclesState(bool state)
    {
        _attackCircleRange.gameObject.SetActive(state);
        _viewCircleRange.gameObject.SetActive(state);
    }
}
