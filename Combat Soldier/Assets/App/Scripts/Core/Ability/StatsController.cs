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

        public StatsController(TroopScriptable config, AIPath aiPath = null)
        {
            _aiPath = aiPath;

            _stats[StatType.MoveSpeed] = new Stat(config.Speed);
            _stats[StatType.ReloadSpeed] = new Stat(config.TimeToReloadAttack);
            _stats[StatType.Damage] = new Stat(config.AttackDamage);
            _stats[StatType.BlockRate] = new Stat(config.BlockRate);

            RefreshStats();
        }

        public Result<Stat> GetStat(StatType statType)
        {
            if (_stats.TryGetValue(statType, out var stat))
                return Result<Stat>.Success(stat);
            
            return Result<Stat>.Failure($"[StatsController] Stat {statType} not found!");
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

        public void RefreshStats()
        {
            if (_aiPath == null || !_stats.ContainsKey(StatType.MoveSpeed))
                return;
            
            _aiPath.maxSpeed = GetStatValue(StatType.MoveSpeed);
        }
    }
}