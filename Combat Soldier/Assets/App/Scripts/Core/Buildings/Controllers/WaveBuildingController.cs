using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Buildings.Strategies;

namespace App.Scripts.Core.Buildings.Controllers
{
    public class WaveBuildingController : BuildingController
    {
        protected override void InitializeBuildingBehaviour()
        {
            _buildingAttack = new WaveAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPointList, _rotatingObjectList, _observePoint, _coroutineRunner);
        }
    }
}
