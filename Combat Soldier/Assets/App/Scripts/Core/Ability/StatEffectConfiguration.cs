using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    [CreateAssetMenu(fileName = "Stat Modifier Effect", menuName = "Scriptable Objects/Abilities/Stat Modifier")]
    public class StatEffectConfiguration : ScriptableObject
    {
        public List<Ability> Abilities;
    }
}