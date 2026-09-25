using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Views
{
    public class StatBarView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _valueText;

        public void Initialize(int currentValue, int maxValue)
        {
            _slider.maxValue = maxValue;
            UpdateValue(currentValue, maxValue);
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
