using UnityEngine;
using App.Scripts.Views;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Canvases.ScreenCanvas
{
    public class BuildingScreenCanvasController : MonoBehaviour, IInitializableCanvas
    {
        [SerializeField] private StatBarView _healthBar;
        
        private IStatsController _statsController;
        
        public void Initialize(IStatsController statsController)
        {
            _statsController = statsController;
            
            int maxHealPoint = statsController.GetStatValueInt(StatType.MaxHealPoint);
            _healthBar.Initialize(maxHealPoint);
        }

        public void UpdateHealth(int currentHealth)
        {
            int maxHealPoint = _statsController.GetStatValueInt(StatType.MaxHealPoint);
            _healthBar.UpdateValue(currentHealth, maxHealPoint);
        }

        public void EnableCanvas() => gameObject.SetActive(true);
        public void DisableCanvas() => gameObject.SetActive(false);
    }
}
