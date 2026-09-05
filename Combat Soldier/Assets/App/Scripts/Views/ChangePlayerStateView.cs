using System.Threading.Tasks;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Views
{
    [RequireComponent(typeof(Button))]
    public class ChangePlayerStateView : MonoBehaviour
    {
        [SerializeField] private Image _cooldownImage = default;

        private const float _reloadingDuration = 1f;
        private bool _isReloading = false;

        public void SetupChangeStateButton(PlayerStateController stateController)
        {
            GetComponent<Button>().onClick.AddListener(() => TrySwitchState(stateController));
            _cooldownImage.fillAmount = 1f;
        }

        private void TrySwitchState(PlayerStateController stateController)
        {
            if (_isReloading || stateController == null)
                return;

            if (stateController.TrySwitchToOppositeState())
                _ = ReloadingCoroutine();
        }

        private async Task ReloadingCoroutine()
        {
            _isReloading = true;
            _cooldownImage.fillAmount = 0f;

            float timer = 0;

            while (timer < _reloadingDuration)
            {
                await Task.Yield();

                timer += Time.deltaTime;

                _cooldownImage.fillAmount = Mathf.Clamp01(timer / _reloadingDuration);
            }

            _cooldownImage.fillAmount = 1f;
            _isReloading = false;
        }
    }
}