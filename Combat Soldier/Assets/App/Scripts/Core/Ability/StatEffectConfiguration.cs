using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Enums;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    [CreateAssetMenu(fileName = "Stat Modifier Effect", menuName = "Scriptable Objects/Abilities/Stat Modifier")]
    public class StatEffectConfiguration : ScriptableObject, IAbilityEffect
    {
        [SerializeField] private StatType _statType;
        [SerializeField] private ModifierType _modifierType;
        [SerializeField] private float _value;
        [SerializeField] private float _duration;
        
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
            stat?.RemoveModifiersFromSource(source);
        }
    }
}