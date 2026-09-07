using System.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Core.Ability
{
    public class StatAbility
    {
        public Stat Stat { get; private set; }
        public float Duration { get; private set; }

        public StatAbility(Stat stat, float duration)
        {
            Stat = stat;
            Duration = duration;
        }
        
        public async Task WaitForTheEndOfModifier(StatModifier mod)
        {
            int waitingTimeInMilliseconds = Mathf.RoundToInt(Duration * 1000);
            await Task.Delay(waitingTimeInMilliseconds);
            
            Stat.RemoveModifier(mod);
        }
    }
}