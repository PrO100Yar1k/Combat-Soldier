using App.Scripts.Core.Troops.TroopScripts;

namespace App.Scripts.Core.Ability
{
    public interface IAbilityEffect
    {
        public void Apply(TroopController target, object source);
        public void Remove(TroopController target, object source);
    }
}