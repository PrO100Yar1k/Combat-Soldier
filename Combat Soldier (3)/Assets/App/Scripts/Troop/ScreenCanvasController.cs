using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCanvasController : CanvasController
{
    [SerializeField] private Slider _healPointSlider = default;
    [SerializeField] private Slider _defensePointSlider = default;

    [Space(3)]

    [SerializeField] private TextMeshProUGUI _healPointText = default;
    [SerializeField] private TextMeshProUGUI _defensePointText = default;

    [Space(3)]

    [SerializeField] private Button _attackButton = default;
    [SerializeField] private Button _moveButton = default;

    [SerializeField] private Button _cancelButton = default;

    public override void InitializeCanvas(TroopController troopController)
    {
        _troopController = troopController;
        _troopScriptable = troopController.TroopScriptable;

        AssignMaxSliderValues();
        AssignButtonsListener();
    }

    private void AssignMaxSliderValues()
    {
        _healPointSlider.maxValue = _troopScriptable.maxHealPoint;
        _defensePointSlider.maxValue = _troopScriptable.maxDefencePoint;

        _healPointSlider.value = _healPointSlider.maxValue;
        _defensePointSlider.value = _defensePointSlider.maxValue;
    }

    private void AssignButtonsListener()
    {
        _attackButton.onClick.AddListener(delegate { GameEvents.instance.TroopEnterAnyMode(_troopController, OrderMode.Attack); });
        _moveButton.onClick.AddListener(delegate { GameEvents.instance.TroopEnterAnyMode(_troopController, OrderMode.Move); });

        _cancelButton.onClick.AddListener(delegate { GameEvents.instance.TroopCancelEnteringMode(); });
    }

    public override void EnableCanvas()
    {
        // to do

        _canvasComponent.enabled = true;
    }

    public override void DisableCanvas()
    {
        // to do

        _canvasComponent.enabled = false;
    }

    public void ChangeHealPointSlider(int targetHealPoint)
    {
        targetHealPoint = Mathf.Clamp(targetHealPoint, 0, _troopScriptable.maxHealPoint);
        _healPointSlider.value = targetHealPoint;

        _healPointText.text = $"{targetHealPoint}";
    }

    public void ChangeDefensePointSlider(int targetDefensePoint)
    {
        targetDefensePoint = Mathf.Clamp(targetDefensePoint, 0, _troopScriptable.maxDefencePoint);
        _defensePointSlider.value = targetDefensePoint;

        _defensePointText.text = $"{targetDefensePoint}";
    }

    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);
}