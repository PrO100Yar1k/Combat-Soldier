using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    public class Stat
    {
        private readonly List<StatModifier> _modifiers = new();
        public float Value { get; private set; }
        
        public Stat(float value)
        {
            Value = value;
        }

        public void ChangeValue()
        {
            float finalValue = Value;
            float sumPercent = 0;

            foreach (var mod in _modifiers)
            {
                if (mod.Type == ModifierType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == ModifierType.PercentMultiplier)
                {
                    sumPercent += mod.Value;
                }

                Value = finalValue * (1 + sumPercent);
            }
        }

        public void AddModifier(StatModifier mod)
        {
            _modifiers.Add(mod);
        } 
        
        public void RemoveModifier(StatModifier mod)
        {
            _modifiers.Remove(mod);
        }
        
        public void RemoveModifier(object source)
        {
            _modifiers.RemoveAll(m => m.Source == source);
        }
    }
}