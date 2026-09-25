using System.Threading.Tasks;
using App.Scripts.Core.HPControllers;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Views
{
    [RequireComponent(typeof(Button))]
    public class StateIconView : MonoBehaviour
    {
        [SerializeField] private Image _stateIconImage;
        [SerializeField] private Image _cooldownImage;

        private readonly float _reloadingDuration = 1f;
        private bool _isReloading;

        public void SetStateIcon(Sprite icon)
        {
            if (_stateIconImage == null || icon == null)
                return;

            _stateIconImage.sprite = icon;
        }
        
        public void SetupChangeStateButton(ISwitchableOppositeState switchableOppositeState)
        {
            GetComponent<Button>().onClick.AddListener(() => TrySwitchState(switchableOppositeState));
            _cooldownImage.fillAmount = 1f;
        }

        private void TrySwitchState(ISwitchableOppositeState switchableOppositeState)
        {
            if (_isReloading || switchableOppositeState == null)
                return;

            if (switchableOppositeState.TrySwitchToOppositeState())
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