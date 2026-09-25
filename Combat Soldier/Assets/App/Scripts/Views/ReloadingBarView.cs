using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Core.Canvases.ScreenCanvas
{
    public class ReloadingBarView : MonoBehaviour
    {
        [SerializeField] private Slider _reloadingSlider;
        
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
    }
}