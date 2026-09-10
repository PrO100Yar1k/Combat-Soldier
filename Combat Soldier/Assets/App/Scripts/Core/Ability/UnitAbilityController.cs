using System.Collections.Generic;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public class UnitAbilityController : MonoBehaviour
    {
        [SerializeField] private List<StatEffectConfiguration> _initialAbilities;
        
        private readonly List<AbilityRuntime> _abilities = new();
        public IReadOnlyList<AbilityRuntime> Abilities => _abilities;

        public void Initialize(TroopController troop, ICoroutineRunner runner)
        {
            foreach (var config in _initialAbilities)
            {
                //_abilities.Add(new AbilityRuntime(config, troop, runner));
            }
        }
        
        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].Tick(deltaTime);
            }
        }
    }
}