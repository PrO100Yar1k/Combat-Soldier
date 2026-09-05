using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.Base;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.State_Machine.Death_State
{
    public abstract class TroopDeathState : TroopBaseState // maybe remove this class
    {
        protected override string StateIconLocation
            => "State Icons/Death-State-Icon";

        protected TroopDeathState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
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
