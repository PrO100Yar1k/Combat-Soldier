using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    public class BuildingStatsController : StatsController<BuildingScriptable>
    {
        public BuildingStatsController(BuildingScriptable data) : base(data)
        {
            
        }

        protected override void InitializeData(BuildingScriptable config)
        {
            _stats[StatType.MaxHealPoint] = new Stat(config.MaxHealPoint);
            _stats[StatType.AttackDamage] = new Stat(config.Damage);
            _stats[StatType.ReloadingAttack] = new Stat(config.ReloadingTime);
            _stats[StatType.ReloadingWave] = new Stat(config.TimeBetweenWaves);
            _stats[StatType.AttackRangeRadius] = new Stat(config.AttackRange);
            _stats[StatType.AttackWaveCount] = new Stat(config.AttackWaveCount);
        }
    }
}