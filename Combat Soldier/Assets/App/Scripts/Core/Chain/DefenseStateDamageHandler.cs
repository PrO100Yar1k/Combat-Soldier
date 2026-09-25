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
            if (context.IsInDefenseState && _defenseModel.CurrentDefense > 0)
            {
                int blockedHealth = Mathf.RoundToInt(context.IncomingDamage * _blockRate);
                int remainingDamage = context.IncomingDamage - blockedHealth;

                int actualBlocked = _defenseModel.AbsorbDamage(blockedHealth);
                int unabsorbedBlock = blockedHealth - actualBlocked; 
                
                context.DamageToHealth = remainingDamage + unabsorbedBlock;
            }

            base.Handle(context);
        }
    }
}