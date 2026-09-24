using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.MVP;

namespace App.Scripts.Core.Canvases.WorldCanvas
{
    public class WorldCanvasModel : ModelBase
    {
        private readonly IStatsController _statsController;
        public float AttackRange { get; private set; }
        public float ViewRange { get; private set; }

        public WorldCanvasModel(IStatsController statsController)
        {
            _statsController = statsController;
            
            AttackRange = _statsController.GetStatValue(StatType.AttackRangeRadius);
            ViewRange = _statsController.GetStatValue(StatType.ViewRangeRadius);
        }

        public void UpdateFromStats()
        {
            AttackRange = _statsController.GetStatValue(StatType.AttackRangeRadius);
            ViewRange = _statsController.GetStatValue(StatType.ViewRangeRadius);
        }

        public override void ResetState()
        {
            AttackRange = 0f;
            ViewRange = 0f;
        }
    }
}