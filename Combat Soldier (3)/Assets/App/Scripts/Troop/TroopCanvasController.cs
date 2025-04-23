using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopCanvasController : MonoBehaviour
{
    [SerializeField] private Slider _healPointSlider = default;
    [SerializeField] private Slider _defensePointSlider = default;

    [SerializeField] private TextMeshProUGUI _healPointText = default;
    [SerializeField] private TextMeshProUGUI _defensePointText = default;

    private TroopScriptable _troopScriptable;

    public void InitializeScriptableObject(TroopScriptable troopScriptable)
    {
        _troopScriptable = troopScriptable;
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
