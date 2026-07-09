using Assets.App.Scripts;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerScreenCanvasController : TroopScreenCanvasController
{
    [SerializeField, Space(3)] private Slider _reloadingSlider = default;

    [SerializeField, Space(3)] private Toggle _disableCanvasToggle = default;

    [SerializeField, Space(3)] private Button _attackButton = default;

    [SerializeField] private Button _moveButton = default;

    [SerializeField] private Button _cancelButton = default;

    public bool DisableCanvasAfterOrder => _disableCanvasToggle.isOn;

    private GameEvents _gameEvents = default;

    [Inject]
    public void Construct(GameEvents gameEvents)
    {
        _gameEvents = gameEvents;
    }

    protected override void AssignDefaultCanvasValues()
    {
        base.AssignDefaultCanvasValues();

        _attackButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Attack); });
        _moveButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Move); });

        _cancelButton.onClick.AddListener(AddEventOnCancelButton);
    }

    #region Button Events

    private void AddEventOnActionButtons(OrderMode orderMode)
    {
        _gameEvents.TroopEnterAnyMode(_troopController, orderMode);
        _cancelButton.gameObject.SetActive(true);
    }

    private void AddEventOnCancelButton()
    {
        _gameEvents.TroopCancelEnteringMode();
        _cancelButton.gameObject.SetActive(false);
    }

    #endregion

    #region Reloading Bar

    public void UpdateReloadingBar(float timeToReload)
        => _ = UpdateReloadingSliderAsync(timeToReload);

    private async Task UpdateReloadingSliderAsync(float timeToReload)
    {
        float timeToCompleteReload = timeToReload;

        _reloadingSlider.value = 0f;
        _reloadingSlider.maxValue = timeToCompleteReload;

        float timeCounter = 0f;

        while (timeCounter < timeToCompleteReload)
        {
            timeCounter += Time.deltaTime;

            _reloadingSlider.value = timeCounter;

            await Task.Yield();
        }

        _reloadingSlider.value = timeToCompleteReload;
    }

    #endregion 

    #region Extra Methods

    public void ChangeCancelButtonState(bool state)
        => _cancelButton.gameObject.SetActive(state);

    public override void EnableCanvas()
    {
        _cancelButton.gameObject.SetActive(false);
        base.EnableCanvas();
    }

    #endregion
}
