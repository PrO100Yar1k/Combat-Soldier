using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.StateMachine.Death_State
{
    public class EnemyDeathState : TroopDeathState
    {
        public EnemyDeathState(TargetSearchService targetSearchService, TroopController troopController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, switcherState, animatorController)
        {

        }
    }
}
