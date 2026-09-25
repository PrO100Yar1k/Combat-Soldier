using System;
using App.Scripts.MVP;
using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class DefenseModel : ModelBase
    {
        public int CurrentDefense { get; private set; }
        public int MaxDefense { get; private set; }

        public event Action<int, int> OnDefenseChanged;

        public DefenseModel(int initialDefense, int maxDefense)
        {
            CurrentDefense = initialDefense;
            MaxDefense = Mathf.Max(0, maxDefense);
        }

        public override void ResetState()
        {
            CurrentDefense = MaxDefense;
            OnDefenseChanged?.Invoke(CurrentDefense, MaxDefense);
        }
        
        public int AbsorbDamage(int amount)
        {
            int absorbed = Mathf.Min(CurrentDefense, amount);
            
            CurrentDefense -= absorbed;
            OnDefenseChanged?.Invoke(CurrentDefense, MaxDefense);
            
            return absorbed;
        }
    }
}