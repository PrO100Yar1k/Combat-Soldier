using App.Scripts.Core.Ability;
using App.Scripts.Core.Buildings.Base;

namespace App.Scripts.Core.HPControllers
{
    public class BuildingHealthComponent : UnitHealthComponent<BuildingController>
    {
        public BuildingHealthComponent(BuildingController controller, IStatsController statsController, ScreenStatsCanvasView screenStatsCanvasView)
            : base(controller, statsController, screenStatsCanvasView)
        {
            
        }
    }
}