using UnityEngine;
using System.Collections.Generic;
using App.Scripts.Infrastructure.Enums;
using App.Scripts.Infrastructure.Others;

namespace App.Scripts.Core.Ability
{
    public abstract class StatsController<TData> : IStatsController where TData : ScriptableObject
    {
        protected readonly Dictionary<StatType, Stat> _stats = new();

        public StatsController(TData data)
        {
            InitializeData(data);
        }
        
        protected abstract void InitializeData(TData config);
        
        public float GetStatValueFloat(StatType statType)
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
            float floatValue = GetStatValueFloat(statType);
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
        /*
        
        private void RefreshSpeedStats() // transmit this method to another script
        {
            if (_aiPath == null || !_stats.ContainsKey(StatType.MaxSpeed))
                return;
            
            //_aiPath.speed = GetStatValue(StatType.MoveSpeed);
            _aiPath.maxSpeed = GetStatValue(StatType.MaxSpeed);
        } */
    }
}