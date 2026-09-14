using UnityEngine;
using UnityEngine.UI;
using App.Scripts.Views;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Canvases.ScreenCanvas
{
    public class TroopScreenCanvasController : MonoBehaviour, IInitializableCanvas
    {
        [SerializeField] private StatBarView _healthBar;
        [SerializeField] private StatBarView _defenseBar;

        [SerializeField] private Image _stateIcon;

        private IStatsController _statsController;

        public virtual void Initialize(IStatsController statsController)
        {
            _statsController = statsController;
            
            int maxHealPoint = statsController.GetStatValueInt(StatType.MaxHealPoint);
            int maxDefensePoint = statsController.GetStatValueInt(StatType.MaxDefensePoint);
            
            _healthBar.Initialize(maxHealPoint);
            _defenseBar.Initialize(maxDefensePoint);
        }

        public void UpdateHealth(int currentHealth)
        {
            int maxHealPoint = _statsController.GetStatValueInt(StatType.MaxHealPoint);
            _healthBar.UpdateValue(currentHealth, maxHealPoint);
        }

        public void UpdateDefense(int currentDefense)
        {
            int maxDefensePoint = _statsController.GetStatValueInt(StatType.MaxDefensePoint);
            _defenseBar.UpdateValue(currentDefense, maxDefensePoint);
        }

        public void ChangeStateIcon(Sprite icon)
        {
            _stateIcon.sprite = icon;
        }

        public virtual void EnableCanvas() => gameObject.SetActive(true);
        public virtual void DisableCanvas() => gameObject.SetActive(false);
    }
}
