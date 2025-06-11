using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ScreenCanvasController : CanvasController
{
    [SerializeField] private Slider _healPointSlider = default;
    [SerializeField] private Slider _defensePointSlider = default;

    [Space(3)]

    [SerializeField] private TextMeshProUGUI _healPointText = default;
    [SerializeField] private TextMeshProUGUI _defensePointText = default;

    [Space(3)]

    [SerializeField] private Image _stateIcon = default;

    public override void InitializeCanvas(TroopController troopController)
    {
        _troopController = troopController;
        _troopScriptable = troopController.TroopScriptable;

        AssignMaxSliderValues();
    }

    private void AssignMaxSliderValues()
    {
        _healPointSlider.maxValue = _troopScriptable.MaxHealPoint;
        _defensePointSlider.maxValue = _troopScriptable.MaxDefencePoint;

        _healPointSlider.value = _healPointSlider.maxValue;
        _defensePointSlider.value = _defensePointSlider.maxValue;
    }


    public void ChangeHealPointSlider(int targetHealPoint)
    {
        targetHealPoint = Mathf.Clamp(targetHealPoint, 0, _troopScriptable.MaxHealPoint);
        _healPointSlider.value = targetHealPoint;

        _healPointText.text = $"{targetHealPoint}";
    }

    public void ChangeDefensePointSlider(int targetDefensePoint)
    {
        targetDefensePoint = Mathf.Clamp(targetDefensePoint, 0, _troopScriptable.MaxDefencePoint);
        _defensePointSlider.value = targetDefensePoint;

        _defensePointText.text = $"{targetDefensePoint}";
    }

    public void ChangeStateIcon(Sprite targetSprite)
        => _stateIcon.sprite = targetSprite;
}