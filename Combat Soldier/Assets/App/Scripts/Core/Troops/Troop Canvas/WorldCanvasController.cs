using UnityEngine;
using UnityEngine.UI;

public abstract class WorldCanvasController : TroopCanvasController
{
    [SerializeField] protected Image _unitCircleRange = default;

    [SerializeField] protected RectTransform _attackCircleRange = default;
    [SerializeField] protected RectTransform _viewCircleRange = default;

    protected override void AssignDefaultCanvasValues()
    {
        ChangeCirclesState(false);
        SetupCircleRanges();
    }

    public override void EnableCanvas()
    {
        ChangeCirclesState(true);
    }

    public override void DisableCanvas()
    {
        ChangeCirclesState(false);
    }

    private void SetupCircleRanges()
    {
        float attackRangeRadius = _troopScriptable.AttackRangeRadius;
        float viewRangeRadius = _troopScriptable.ViewRangeRadius;

        _attackCircleRange.localScale = new Vector2(attackRangeRadius * 2, attackRangeRadius * 2);
        _viewCircleRange.localScale = new Vector2(viewRangeRadius * 2, viewRangeRadius * 2);
    }

    private void ChangeCirclesState(bool state)
    {
        _attackCircleRange.gameObject.SetActive(state);
        _viewCircleRange.gameObject.SetActive(state);

        _unitCircleRange.gameObject.SetActive(!state);
    }
}
