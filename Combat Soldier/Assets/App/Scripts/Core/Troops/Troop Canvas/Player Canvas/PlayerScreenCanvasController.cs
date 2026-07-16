using System.Threading.Tasks;
using Assets.App.Scripts;
using UnityEngine.UI;
using UnityEngine;
using Zenject;

public class PlayerScreenCanvasController : TroopScreenCanvasController
{
    [SerializeField, Space(3)] private Slider _reloadingSlider = default;

    [SerializeField] private Button _uniteArmyButton = default;
    [SerializeField] private Button _splitArmyButton = default;

    public bool DisableCanvasAfterOrder => true;

    private GameEventBus _gameEvents = default;

    [Inject]
    public void Construct(GameEventBus gameEvents)
    {
        _gameEvents = gameEvents;
    }

    protected override void AssignDefaultCanvasValues()
    {
        base.AssignDefaultCanvasValues();

        _uniteArmyButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Unite); });
        _splitArmyButton.onClick.AddListener(delegate { AddEventOnActionButtons(OrderMode.Split); });
    }

    #region Button Events

    private void AddEventOnActionButtons(OrderMode orderMode)
    {
        _gameEvents.TroopEnterAnyMode(_troopController, orderMode);
    }

    #endregion

    #region Reloading Bar

    public void UpdateReloadingBar(float timeToReload)
    {
        _ = UpdateReloadingSliderAsync(timeToReload);
    }

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
}
