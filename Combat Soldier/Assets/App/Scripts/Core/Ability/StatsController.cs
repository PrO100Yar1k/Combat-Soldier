using System.Collections.Generic;
using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Others;
using Pathfinding;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public class StatsController
    {
        private readonly Dictionary<StatType, Stat> _stats = new();
        private readonly AIPath _aiPath;

        public StatsController(TroopScriptable config, AIPath aiPath)
        {
            _aiPath = aiPath;

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

            RefreshSpeedStats();
        }

        public float GetStatValue(StatType statType)
        {
            var statResult = GetStat(statType);
            if (!statResult.IsSuccess)
            {
                Debug.LogError(statResult.Error);
                return 0;
            }
            
            var stat = statResult.Value;
            return stat.Value;
        }
        
        public int GetStatValueInt(StatType statType)
        {
            float floatValue = GetStatValue(statType);
            int intValue = Mathf.RoundToInt(floatValue);
            
            //Debug.Log($"[StatsController] Round from {floatValue} to {intValue}");
            return intValue;
        }
        
        public Result<Stat> GetStat(StatType statType)
        {
            if (_stats.TryGetValue(statType, out var stat))
                return Result<Stat>.Success(stat);
            
            return Result<Stat>.Failure($"[StatsController] Stat {statType} not found!");
        }
        
        private void RefreshSpeedStats()
        {
            if (_aiPath == null || !_stats.ContainsKey(StatType.MaxSpeed))
                return;
            
            //_aiPath.speed = GetStatValue(StatType.MoveSpeed);
            _aiPath.maxSpeed = GetStatValue(StatType.MaxSpeed);
        }
    }
}