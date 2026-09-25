using App.Scripts.Views;
using App.Scripts.MVP;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class ScreenCanvasView : ViewBase
    {
        [SerializeField] private StatBarView _healthBar;
        [SerializeField] private StatBarView _defenseBar;
        
        public void InitializeHealthBar(int currentHealth, int maxHealth)
        {
            _healthBar.Initialize(currentHealth, maxHealth);
        }
        
        public void InitializeDefenseBar(int currentDefense, int maxDefense)
        {
            _defenseBar.Initialize(currentDefense, maxDefense);
        }
        
        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            _healthBar.UpdateValue(currentHealth, maxHealth);
        }

        public void UpdateDefense(int currentDefense, int maxDefense)
        {
            _defenseBar.UpdateValue(currentDefense, maxDefense);
        }
    }
}