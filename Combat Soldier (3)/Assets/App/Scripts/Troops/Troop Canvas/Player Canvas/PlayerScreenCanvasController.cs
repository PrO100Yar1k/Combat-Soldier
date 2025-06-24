using UnityEngine;
using UnityEngine.UI;

public class PlayerScreenCanvasController : TroopScreenCanvasController
{
    [Space(3)]

    [SerializeField] private Button _attackButton = default;
    [SerializeField] private Button _moveButton = default;

    [SerializeField] private Button _cancelButton = default;

    protected override void AssignDefaultCanvasValues()
    {
        base.AssignDefaultCanvasValues();

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

        base.EnableCanvas();
    }

    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);
}
