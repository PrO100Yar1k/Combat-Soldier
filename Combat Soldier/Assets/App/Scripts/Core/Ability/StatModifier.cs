using System;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.Ability
{
    [Serializable]
    public class StatModifier
    {
        public ModifierType Type { get; }
        public float Value { get; }
        
        public object Source { get; } //

        public StatModifier(ModifierType type, float value, object source)
        {
            Type = type;
            Value = value;
            Source = source;
        }
    }
}