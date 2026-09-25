using System.Collections;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.MVP;
using App.Scripts.Views;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Core.Canvases.WorldCanvas
{
    public class WorldCanvasView : ViewBase
    {
        [SerializeField] private WorldRangeView _rangeView;

        [SerializeField] private Image _unitCircleBar;
        [SerializeField] private Image _unitReloadingCircleBar;
        [SerializeField] private GameObject _unitCircleLining;

        private Coroutine _reloadingCoroutine;
        private Coroutine _damageEffectCoroutine;
        
        private ICoroutineRunner _coroutineRunner;
        
        public void SetupRunner(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void SetupRanges(float attackRange, float viewRange)
        {
            _rangeView.SetupRanges(attackRange, viewRange);
        }

        public void SetRangesActive(bool active)
        { 
            _rangeView.SetCirclesActive(active);
        }

        #region Reloading Animation

        public void StartReloading(float reloadingTime)
        {
            if (_reloadingCoroutine != null)
                _coroutineRunner.StopCoroutine(_reloadingCoroutine);

            _reloadingCoroutine = _coroutineRunner.StartCoroutine(ReloadingCoroutine(reloadingTime));
        }

        private IEnumerator ReloadingCoroutine(float reloadingTime)
        {
            float elapsedTime = 0f;
            SetReloadingUIState(true);

            while (elapsedTime < reloadingTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / reloadingTime);

                _unitCircleBar.fillAmount = progress;
                _unitReloadingCircleBar.fillAmount = progress;

                yield return null;
            }

            SetReloadingUIState(false);
        }

        private void SetReloadingUIState(bool isReloading)
        {
            _unitReloadingCircleBar.fillAmount = isReloading ? 0f : 1f;
            _unitCircleLining.SetActive(isReloading);
            _unitReloadingCircleBar.gameObject.SetActive(isReloading);
        }

        #endregion

        #region Damage Animation Effect

        public void PlayDamageEffect()
        {
            if (_damageEffectCoroutine != null)
                _coroutineRunner.StopCoroutine(_damageEffectCoroutine);
            
            _damageEffectCoroutine = _coroutineRunner.StartCoroutine(DamageEffectCoroutine());
        }

        private IEnumerator DamageEffectCoroutine()
        {
            float loopDelay = 1f;
            SetUnitRangeAlpha(140);
            yield return new WaitForSeconds(loopDelay / 2);
            SetUnitRangeAlpha(220);
        }

        private void SetUnitRangeAlpha(byte alpha)
        {
            Color32 currentColor= _unitCircleBar.color;
            _unitCircleBar.color= new Color32(currentColor.r, currentColor.g, currentColor.b, alpha);
        }
        
        public void SetInsideViewRange()
        {
            SetUnitRangeAlpha(180);
        }

        public void SetOutsideViewRange()
        {
            StopReloadingAnimation();
            DisableReloadingUI();
            SetUnitRangeAlpha(85);
        }

        public void StopReloadingAnimation()
        {
            if (_reloadingCoroutine == null)
                return;
            
            _coroutineRunner.StopCoroutine(_reloadingCoroutine);
            _reloadingCoroutine = null;
        }

        private void DisableReloadingUI()
        {
            _unitCircleLining.gameObject.SetActive(false); 
            _unitReloadingCircleBar.gameObject.SetActive(false);
        }

        #endregion
    }
}