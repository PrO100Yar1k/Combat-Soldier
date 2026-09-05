using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.State_Machine.Death_State
{
    public class EnemyDeathState : TroopDeathState
    {
        public EnemyDeathState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
        {

        }
    }
}
