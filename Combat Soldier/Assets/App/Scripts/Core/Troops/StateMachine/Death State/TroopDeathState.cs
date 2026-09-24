using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.Base;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.StateMachine.Death_State
{
    public abstract class TroopDeathState : TroopBaseState // maybe remove this class
    {
        protected override string StateIconLocation
            => "State Icons/Death-State-Icon";

        protected TroopDeathState(TargetSearchService targetSearchService, TroopController troopController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, switcherState, animatorController)
        {

        }

        public override void OnStart()
        {
            PlayStateAnimation();
        }

        public override void OnStop()
        {

        }

        protected override void PlayStateAnimation()
        {
            //_animatorController.PlayDeath();
        }
    }
}
