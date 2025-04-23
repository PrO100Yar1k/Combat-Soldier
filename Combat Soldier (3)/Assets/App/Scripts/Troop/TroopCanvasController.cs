using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopCanvasController : MonoBehaviour
{
    [SerializeField] private Slider _healPointSlider = default;
    [SerializeField] private Slider _defensePointSlider = default;

    [Space(3)]

    [SerializeField] private TextMeshProUGUI _healPointText = default;
    [SerializeField] private TextMeshProUGUI _defensePointText = default;

    private TroopScriptable _troopScriptable;

    public void InitializeScriptableObject(TroopScriptable troopScriptable)
    {
        _troopScriptable = troopScriptable;

        AssignMaxSliderValues();
    }

    private void AssignMaxSliderValues()
    {
        _healPointSlider.maxValue = _troopScriptable.maxHealPoint;
        _defensePointSlider.maxValue = _troopScriptable.maxDefencePoint;

        _healPointSlider.value = _healPointSlider.maxValue;
        _defensePointSlider.value = _defensePointSlider.maxValue;
    }

    public void ChangeHealPointSlider(int targetHealPoint)
    {
        if (_troopScriptable == null)
            return;

        targetHealPoint = Mathf.Clamp(targetHealPoint, 0, _troopScriptable.maxHealPoint);
        _healPointSlider.value = targetHealPoint;

        _healPointText.text = $"{targetHealPoint}";
    }

    public void ChangeDefensePointSlider(int targetDefensePoint)
    {
        if (_troopScriptable == null)
            return;

        targetDefensePoint = Mathf.Clamp(targetDefensePoint, 0, _troopScriptable.maxDefencePoint);
        _defensePointSlider.value = targetDefensePoint;

        _defensePointText.text = $"{targetDefensePoint}";
    }
}
