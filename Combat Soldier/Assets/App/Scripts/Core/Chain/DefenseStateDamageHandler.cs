using UnityEngine;

namespace App.Scripts.Core.HPControllers
{
    public class DefenseStateDamageHandler : BaseDamageHandler
    {
        private readonly DefenseModel _defenseModel;
        private readonly float _blockRate;

        public DefenseStateDamageHandler(DefenseModel defenseModel, float blockRate)
        {
            _defenseModel = defenseModel;
            _blockRate = blockRate;
        }

        public override void Handle(DamageContext context)
        {
            if (context.IsInDefenseState && _defenseModel != null && _defenseModel.CurrentDefense > 0)
            {
                int blockedHP = Mathf.RoundToInt(context.IncomingDamage * _blockRate);
                int remainingDamage = context.IncomingDamage - blockedHP;

                int actualBlocked = _defenseModel.AbsorbDamage(blockedHP);
                
                // Шкода, яку не зміг ввібрати щит, іде в здоров'я
                int unabsorbedBlock = blockedHP - actualBlocked; 
                context.DamageToHealth = remainingDamage + unabsorbedBlock;
            }

            base.Handle(context);
        }
    }
}