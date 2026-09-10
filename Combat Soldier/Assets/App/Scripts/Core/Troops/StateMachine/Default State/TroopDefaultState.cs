using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.Base;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.StateMachine.Default_State
{
    public abstract class TroopDefaultState : TroopBaseState
    {
        protected override string StateIconLocation
            => "State Icons/Default-State-Icon";

        protected TroopDefaultState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
        {

        }

        protected override void PlayStateAnimation()
        {
            _animatorController.PlayIdle();
        }
    }
}
