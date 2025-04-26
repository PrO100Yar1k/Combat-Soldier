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
        _troopScriptable = troopController.TroopScriptable;
        _troopController = troopController;

        AssignMaxSliderValues();
        AssignButtonsListener();
    }

    private void AssignMaxSliderValues()
    {
        _healPointSlider.maxValue = _troopScriptable.MaxHealPoint;
        _defensePointSlider.maxValue = _troopScriptable.MaxDefencePoint;

        _healPointSlider.value = _healPointSlider.maxValue;
        _defensePointSlider.value = _defensePointSlider.maxValue;
    }

    private void AssignButtonsListener()
    {
        _attackButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Attack); });
        _moveButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Move); });

        _cancelButton.onClick.AddListener(AddEventOnCancelButton);
    }

    private void AddEventOnActionButtons(OrderMode orderMode)
    {
        GameEvents.instance.TroopEnterAnyMode(_troopController, orderMode);
        _cancelButton.gameObject.SetActive(true);
    }

    private void AddEventOnCancelButton()
    {
        GameEvents.instance.TroopCancelEnteringMode();
        _cancelButton.gameObject.SetActive(false);
    }


    public override void EnableCanvas()
    {
        _cancelButton.gameObject.SetActive(false);
        _canvasComponent.enabled = true;
    }

    public override void DisableCanvas()
    {
        _canvasComponent.enabled = false;
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

    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);
}