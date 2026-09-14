using UnityEngine;
using App.Scripts.Core.Ability;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Core.Canvases.ScreenCanvas;

namespace App.Scripts.Core.HPControllers
{
    public class HPBuildingController : HPController
    {
        private readonly BuildingScreenCanvasController _buildingCanvasController;
        private readonly BuildingController _buildingController;

        public HPBuildingController(BuildingController buildingController, BuildingScreenCanvasController buildingCanvasController)
        {
            _buildingController = buildingController;
            _buildingCanvasController = buildingCanvasController;

            InitializeData(_buildingController.StatsController);
            UpdateSliderAndTextValues();
        }

        protected override void InitializeData(IStatsController statsController)
        {
            _unitName = _buildingController.BuildingScriptable.Name;
            _currentHealPoint = statsController.GetStatValueInt(StatType.MaxHealPoint);
        }

        protected override void UpdateSliderAndTextValues()
        {
            _buildingCanvasController.UpdateHealth(_currentHealPoint);
        }

        public override void TakeDamage(int attackDamage)
        {
            _currentHealPoint -= attackDamage;

            UpdateSliderAndTextValues();
            CheckHealPointsForDeath();
        }

        protected override void HandleDeath()
        {
            _buildingController.Dispose();
            _buildingController.StopAllCoroutines();

            UnityEngine.Object.Destroy(_buildingController.gameObject);
            Debug.Log($"The {_unitName} was died");
        }
    }
}
