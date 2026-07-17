using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class WorldCanvasController : TroopCanvasController
{
    [SerializeField] protected Image _unitCircleRange = default;
    [SerializeField] protected Image _unitReloadingCircleRange = default;

    [SerializeField] protected RectTransform _viewCircleRange = default;
    [SerializeField] protected RectTransform _attackCircleRange = default;

    [SerializeField] protected RectTransform _unitCircleLining = default;

    protected override void AssignDefaultCanvasValues()
    {
        SetupCircleRanges();
    }

    public override void EnableCanvas()
    {
        ChangeCirclesState(true);
    }

    public override void DisableCanvas()
    {
        ChangeCirclesState(false);
        ChangeReloadingCirclesState(false);
    }

    public void ChangeUnitCircleToReloading(float reloadingTime)
    {
        _troopController.StartCoroutine(ChangeUnitCircleToReloadingCoroutine(reloadingTime));
    }

    public void ChangeUnitCircleUnderAttack()
    {
        _troopController.StartCoroutine(ChangeUnitCircleUnderAttackCoroutine());
    }

    private void SetupCircleRanges()
    {
        float attackRangeRadius = _troopScriptable.AttackRangeRadius;
        float viewRangeRadius = _troopScriptable.ViewRangeRadius;

        _attackCircleRange.sizeDelta = new Vector2(attackRangeRadius * 2, attackRangeRadius * 2);
        _viewCircleRange.sizeDelta = new Vector2(viewRangeRadius * 2, viewRangeRadius * 2);
    }

    private void ChangeCirclesState(bool state)
    {
        _attackCircleRange.gameObject.SetActive(state);
        _viewCircleRange.gameObject.SetActive(state);

        _unitCircleRange.gameObject.SetActive(!state);
    }

    private IEnumerator ChangeUnitCircleToReloadingCoroutine(float reloadingTime)
    {
        float elapsedTime = 0f;

        SetupReloadingBars(true);

        while (elapsedTime < reloadingTime)
        {
            elapsedTime += Time.deltaTime;

            _unitCircleRange.fillAmount = Mathf.Clamp01(elapsedTime / reloadingTime);
            _unitReloadingCircleRange.fillAmount = Mathf.Clamp01(elapsedTime / reloadingTime);

            yield return null;
        }

        SetupReloadingBars(false);
    }

    private void SetupReloadingBars(bool condition)
    {
        _unitReloadingCircleRange.fillAmount = condition ? 0f : 1f;

        ChangeReloadingCirclesState(condition);
    }

    private void ChangeReloadingCirclesState(bool condition)
    {
        _unitCircleLining.gameObject.SetActive(condition);
        _unitReloadingCircleRange.gameObject.SetActive(condition);
    }

    private IEnumerator ChangeUnitCircleUnderAttackCoroutine()
    {
        float loopDelay = 0.7f;

        SetupUnitRange(140);
        yield return new WaitForSeconds(loopDelay / 2);

        SetupUnitRange(220);
        yield return new WaitForSeconds(loopDelay / 2);
    }

    protected void SetupUnitRange(byte alphaColor)
    {
        Color32 currentColor = _unitCircleRange.color;
        _unitCircleRange.color = new Color32(currentColor.r, currentColor.g, currentColor.b, alphaColor);
    }
}
