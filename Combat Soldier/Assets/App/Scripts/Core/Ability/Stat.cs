using System.Collections.Generic;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    public class Stat
    {
        private readonly List<StatModifier> _modifiers = new();
        private readonly float _baseValue;

        public float Value { get; private set; }
        
        public Stat(float baseValue)
        {
            _baseValue = baseValue;
            CalculateModifierEffect();
        }

        public void CalculateModifierEffect()
        {
            float flatBonus = 0;
            float percentBonus = 0;

            foreach (var mod in _modifiers)
            {
                if (mod.Type == ModifierType.Flat)
                {
                    flatBonus += mod.Value;
                }
                else if (mod.Type == ModifierType.PercentMultiplier)
                {
                    percentBonus += mod.Value;
                }
            }

            Value = (_baseValue + flatBonus) * (1f + (percentBonus / 100f));
        }

        public void AddModifier(StatModifier mod)
        {
            _modifiers.Add(mod);
            CalculateModifierEffect();
        }

        public void RemoveModifier(object source)
        {
            _modifiers.RemoveAll(m => m.Source == source);
            CalculateModifierEffect();
        }
    }
}