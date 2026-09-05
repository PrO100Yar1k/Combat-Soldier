using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Buildings.Strategies;

namespace App.Scripts.Core.Buildings.Controllers
{
    public class MultiCannonBuildingController : BuildingController
    {
        protected override void InitializeBuildingBehaviour()
        {
            _buildingAttack = new MultiCannonAttackBehaviour(this, _targetSearchService, _buildingScriptable, _bulletInitialPointList, _rotatingObjectList, _observePoint, _coroutineRunner);
        }
    }
}
