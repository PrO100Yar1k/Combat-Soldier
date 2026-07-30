using System.Collections;
using Assets.App.Views;
using UnityEngine.UI;
using UnityEngine;

namespace Assets.App.Scripts.Core.Canvases
{ 
    public class TroopWorldCanvasController : MonoBehaviour, IInitializableCanvas<TroopScriptable>, ICoroutineCanvas
    {
        [SerializeField] protected WorldRangeView _rangeView = default;

        [SerializeField] protected RectTransform _unitCircleLining = default;

        [SerializeField] protected Image _unitCircleRange = default;
        [SerializeField] protected Image _unitReloadingCircleRange = default;

        protected Coroutine _reloadingCoroutine = default;

        protected ICoroutineRunner _coroutineRunner = default;

        protected bool _isReloading = false;

        public void Initialize(TroopScriptable troopData)
        {
            _rangeView.SetupRanges(troopData.AttackRangeRadius, troopData.ViewRangeRadius);
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
            StopReloading();

            _reloadingCoroutine = _coroutineRunner.StartCoroutine(ReloadingCoroutine(reloadingTime));
        }

        public void StartTakingDamage()
        {
            _coroutineRunner.StartCoroutine(TakingDamageCoroutine());
        }

        protected void StopReloading()
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
