using System.Collections.Generic;
using App.Scripts.Core.Troops.TroopScripts;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public class UnitAbilityController : MonoBehaviour
    {
        [SerializeField] private List<StatEffectConfiguration> _initialAbilities;
        
        private readonly List<AbilityRuntime> _abilities = new();
        public IReadOnlyList<AbilityRuntime> Abilities => _abilities;
        
        public void Initialize(TroopController troop)
        {
            foreach (var config in _initialAbilities)
            {
                foreach (var ability in config.Abilities)
                {
                    var abilityRuntime = new AbilityRuntime(ability, troop);
                    _abilities.Add(abilityRuntime);
                    
                    bool result = abilityRuntime.TryActivate();
                    //Debug.Log($"Ability result: {result}");
                }
            }
        }
        
        private void Update() //
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].Tick(deltaTime);
            }
        }
    }
}