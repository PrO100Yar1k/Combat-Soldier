using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Core.Troops.StateMachine.State_Controller;

namespace App.Scripts.Core.Troops.StateMachine.Attack_State
{
    public class PlayerAttackState : TroopAttackState
    {
        protected override Faction TroopSide => Faction.Allies;

        public PlayerAttackState(TargetSearchService targetSearchService, TroopController troopController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, switcherState, animatorController)
        {
            
        }
    }
}
