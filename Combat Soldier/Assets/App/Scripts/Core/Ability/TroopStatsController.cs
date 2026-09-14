using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    public class TroopStatsController : StatsController<TroopScriptable>
    {
        public TroopStatsController(TroopScriptable data) : base(data)
        {
            
        }

        protected override void InitializeData(TroopScriptable config)
        {
            _stats[StatType.MaxHealPoint] = new Stat(config.MaxHealPoint);
            _stats[StatType.MaxDefensePoint] = new Stat(config.MaxDefencePoint);
            _stats[StatType.MoveSpeed] = new Stat(config.Speed);
            _stats[StatType.MaxSpeed] = new Stat(config.MaxSpeed);
            _stats[StatType.AttackDamage] = new Stat(config.AttackDamage);
            _stats[StatType.BlockRate] = new Stat(config.BlockRate);
            _stats[StatType.ReloadingAttack] = new Stat(config.TimeToReloadAttack);
            _stats[StatType.ReloadingWave] = new Stat(config.TimeBetweenAttackWaves);
            _stats[StatType.DamageUnderAttack] = new Stat(config.DamageUnderAttack);
            _stats[StatType.ViewRangeRadius] = new Stat(config.ViewRangeRadius);
            _stats[StatType.AttackRangeRadius] = new Stat(config.AttackRangeRadius);
            _stats[StatType.AttackWaveCount] = new Stat(config.CountAttackWaves);
        }
    }
}