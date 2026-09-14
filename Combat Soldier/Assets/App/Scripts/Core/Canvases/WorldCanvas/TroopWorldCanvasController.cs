using UnityEngine;
using UnityEngine.UI;
using App.Scripts.Views;
using System.Collections;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Canvases.WorldCanvas
{ 
    public class TroopWorldCanvasController : MonoBehaviour, IInitializableCanvas, ICoroutineCanvas
    {
        [SerializeField] protected WorldRangeView _rangeView;

        [SerializeField] protected RectTransform _unitCircleLining;

        [SerializeField] protected Image _unitCircleRange;
        [SerializeField] protected Image _unitReloadingCircleRange;

        private IStatsController _statsController;
        
        private Coroutine _reloadingCoroutine;
        private ICoroutineRunner _coroutineRunner;

        private bool _isReloading;

        public void Initialize(IStatsController statsController)
        {
            _statsController = statsController;
            
            float attackRangeRadius = statsController.GetStatValue(StatType.AttackRangeRadius);
            float viewRangeRadius = statsController.GetStatValue(StatType.ViewRangeRadius);
            
            _rangeView.SetupRanges(attackRangeRadius, viewRangeRadius);
        }

        public void SetupCoroutineRunner(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void EnableCanvas()
        {
            SetupCanvasActivity(true);
        }

        public void DisableCanvas()
        {
            SetupCanvasActivity(false);
        }

        private void SetupCanvasActivity(bool activity)
        {
            _rangeView.SetCirclesActive(activity);
            SetReloadingUIState(_isReloading);
        }

        public void StartReloading(float reloadingTime)
        {
            StopReloadingCoroutine();
            _reloadingCoroutine = _coroutineRunner.StartCoroutine(ReloadingCoroutine(reloadingTime));
        }

        public void StartTakingDamage()
        {
            _coroutineRunner.StartCoroutine(TakingDamageCoroutine());
        }

        protected void StopReloadingCoroutine()
        {
            if (_reloadingCoroutine == null)
                return;

            _coroutineRunner.StopCoroutine(_reloadingCoroutine);
        }

        private IEnumerator ReloadingCoroutine(float reloadingTime)
        {
            float elapsedTime = 0f;

            SetReloadingUIState(true);

            while (elapsedTime < reloadingTime)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / reloadingTime);

                _unitCircleRange.fillAmount = progress;
                _unitReloadingCircleRange.fillAmount = progress;

                yield return null;
            }

            SetReloadingUIState(false);
        }

        private void SetReloadingUIState(bool isReloading)
        {
            _isReloading = isReloading;
            _unitReloadingCircleRange.fillAmount = isReloading ? 0f : 1f;

            _unitCircleLining.gameObject.SetActive(isReloading);
            _unitReloadingCircleRange.gameObject.SetActive(isReloading);
        }

        private IEnumerator TakingDamageCoroutine()
        {
            float loopDelay = 1f;

            SetupUnitRange(140);
            yield return new WaitForSeconds(loopDelay / 2);

            SetupUnitRange(220);
            yield return new WaitForSeconds(loopDelay / 2);
        }

        protected void SetupUnitRange(byte alphaColor)
        {
            Color32 currentColor = _unitCircleRange.color;
            _unitCircleRange.color = new Color32(currentColor.r, currentColor.g, currentColor.b, alphaColor);
        }
    }
}
