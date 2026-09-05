using System.Collections.Generic;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    public class Stat //
    {
        public float BaseValue { get; private set; }
        private readonly List<StatModifier> _modifiers = new List<StatModifier>();

        public float Value
        {
            get
            {
                float finalValue = BaseValue;
                float sumPercent = 0;

                for (int i = 0; i < _modifiers.Count; i++)
                {
                    var mod = _modifiers[i];
                    if (mod.Type == ModifierType.Flat)
                    {
                        finalValue += mod.Value;
                    }
                    else if (mod.Type == ModifierType.PercentMultiplier)
                    {
                        sumPercent += mod.Value;
                    }
                }

                return finalValue * (1 + sumPercent);
            }
        }

        public Stat(float baseValue) => BaseValue = baseValue;

        public void AddModifier(StatModifier mod) => _modifiers.Add(mod);
        public void RemoveModifiersFromSource(object source) => _modifiers.RemoveAll(m => m.Source == source);
    }
}