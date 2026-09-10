using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Enums;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    [System.Serializable]
    public class Ability : IAbilityEffect
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private ModifierType _modifierType;
        [SerializeField] private float _value;
        
        [SerializeField] private float _duration;
        [SerializeField] private float _cooldown;
        
        public void Apply(TroopController target, object source)
        {
            var statResult = target.StatsController.GetStat(_statType);
            
            if (!statResult.IsSuccess)
            {
                Debug.LogError(statResult.Error);
                return;
            }
            
            var stat = statResult.Value;
            stat?.AddModifier(new StatModifier(_modifierType, _value, source));
        }

        public void Remove(TroopController target, object source)
        {
            var statResult = target.StatsController.GetStat(_statType);
            
            if (!statResult.IsSuccess)
            {
                Debug.LogError(statResult.Error);
                return;
            }
            
            var stat = statResult.Value;
            stat?.RemoveModifier(source);
        }
    }
}