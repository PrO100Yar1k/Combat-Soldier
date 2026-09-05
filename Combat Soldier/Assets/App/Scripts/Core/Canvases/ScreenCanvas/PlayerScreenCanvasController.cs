using System.Threading.Tasks;
using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Events;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Core.Canvases.ScreenCanvas
{
    public class PlayerScreenCanvasController : TroopScreenCanvasController
    {
        [SerializeField] private Slider _reloadingSlider = default;

        //[SerializeField] private Button _uniteArmyButton = default;
        //[SerializeField] private Button _splitArmyButton = default;

        public bool DisableCanvasAfterOrder => true;

        private GameEventBus _gameEventBus = default;

        [Inject]
        public void Construct(GameEventBus gameEvents)
        {
            _gameEventBus = gameEvents;
        }

        public override void Initialize(TroopScriptable troopData)
        {
            base.Initialize(troopData);
            SetupActionButtons();
        }

        #region Public Controls

        public void UpdateReloadingBar(float timeToReload)
        {
            _ = UpdateReloadingSliderAsync(timeToReload);
        }

        #endregion

        #region Buttons & Coroutines

        private void SetupActionButtons()
        {
            //_uniteArmyButton.onClick.RemoveAllListeners();
            //_splitArmyButton.onClick.RemoveAllListeners();

            //_uniteArmyButton.onClick.AddListener(() => OnActionButtonClicked(OrderMode.Unite));
            //_splitArmyButton.onClick.AddListener(() => OnActionButtonClicked(OrderMode.Split));
        }

        private void OnActionButtonClicked(OrderMode orderMode)
        {
            // _gameEvents.TroopEnterAnyMode(_troopController, orderMode);
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
}