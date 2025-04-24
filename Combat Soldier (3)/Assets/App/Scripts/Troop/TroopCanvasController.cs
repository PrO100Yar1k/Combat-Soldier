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

    [Space(3)]

    [SerializeField] private RectTransform _attackCircleRange = default;
    [SerializeField] private RectTransform _viewCircleRange = default;

    [Space(3)]

    [SerializeField] private Button _attackButton = default;
    [SerializeField] private Button _moveButton = default;

    [SerializeField] private Button _cancelButton = default;

    private TroopScriptable _troopScriptable;

    public void InitializeTroopCanvas(TroopScriptable troopScriptable, TroopController troopController)
    {
        _troopScriptable = troopScriptable;

        AssignMaxSliderValues();
        AssignButtonsListener(troopController);
    }

    private void AssignMaxSliderValues()
    {
        _healPointSlider.maxValue = _troopScriptable.maxHealPoint;
        _defensePointSlider.maxValue = _troopScriptable.maxDefencePoint;

        _healPointSlider.value = _healPointSlider.maxValue;
        _defensePointSlider.value = _defensePointSlider.maxValue;
    }

    private void AssignButtonsListener(TroopController troopController)
    {
        _attackButton.onClick.AddListener(delegate { GameEvents.instance.TroopEnterAnyMode(troopController, OrderMode.Attack); });
        _moveButton.onClick.AddListener(delegate { GameEvents.instance.TroopEnterAnyMode(troopController, OrderMode.Move); });

        _cancelButton.onClick.AddListener(delegate { GameEvents.instance.TroopCancelEnteringMode(); });
    }

    private void SubscribeToEvents()
    {
        //GameEvents.instance.OnTroopEnterAnyMode += ChangeCancelButtonState;
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

    public void SetupCircleRanges()
    {
        float attackRangeRadius = _troopScriptable.attackRangeRadius;
        float viewRangeRadius = _troopScriptable.viewRangeRadius;

        _attackCircleRange.sizeDelta = new Vector2(attackRangeRadius, attackRangeRadius);
        _viewCircleRange.sizeDelta = new Vector2(viewRangeRadius, viewRangeRadius);
    }

    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);
}