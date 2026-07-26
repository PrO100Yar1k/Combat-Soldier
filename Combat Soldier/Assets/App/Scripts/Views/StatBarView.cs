using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.App.Scripts
{
    public class StatBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider = default;
        [SerializeField] private TextMeshProUGUI _valueText = default;

        public void Initialize(int maxValue)
        {
            _slider.maxValue = maxValue;
            _slider.value = maxValue;

            UpdateText(maxValue);
        }

        public void UpdateValue(int currentValue, int maxValue)
        {
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
            _slider.value = currentValue;

            UpdateText(currentValue);
        }

        private void UpdateText(int value)
        {
            _valueText.text = value.ToString();
        }
    }
}
