using App.Scripts.Core.Troops.Troop_Scripts;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public interface IAbilityEffect
    {
        public void Apply(TroopController target, object source);
        public void Remove(TroopController target, object source);
    }
}