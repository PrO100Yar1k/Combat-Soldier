using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Views;
using UnityEngine;

namespace App.Scripts.Core.Canvases.ScreenCanvas
{
    public class BuildingScreenCanvasController : MonoBehaviour, IInitializableCanvas<BuildingScriptable>
    {
        [SerializeField] private StatBarView _healthBar = default;

        private BuildingScriptable _buildingData = default;

        public void Initialize(BuildingScriptable buildingData)
        {
            _buildingData = buildingData;
            _healthBar.Initialize(_buildingData.MaxHealPoint);
        }

        public void UpdateHealth(int currentHealth)
        {
            _healthBar.UpdateValue(currentHealth, _buildingData.MaxHealPoint);
        }

        public void EnableCanvas() => gameObject.SetActive(true);
        public void DisableCanvas() => gameObject.SetActive(false);
    }
}
