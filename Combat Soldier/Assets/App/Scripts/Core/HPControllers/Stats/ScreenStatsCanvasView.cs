using App.Scripts.Views;
using App.Scripts.MVP;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class ScreenStatsCanvasView : ViewBase
    {
        [SerializeField] private StatBarView _healthBar;
        [SerializeField] private StatBarView _defenseBar;

        public void InitializeHealthBar(int maxValue, int? currentValue = null)
        {
            _healthBar.Initialize(maxValue, currentValue);
        }

        public void InitializeDefenseBar(int maxValue, int? currentValue = null)
        { 
            _defenseBar.Initialize(maxValue, currentValue);
        }

        public void UpdateHealth(int current, int max)
        {
            _healthBar.UpdateValue(current, max);
        }

        public void UpdateDefense(int current, int max)
        {
            _defenseBar.UpdateValue(current, max);
        }
    }
}