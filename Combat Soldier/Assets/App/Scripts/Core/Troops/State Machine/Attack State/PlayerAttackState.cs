using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Infrastructure.Others;

namespace App.Scripts.Core.Troops.State_Machine.Attack_State
{
    public class PlayerAttackState : TroopAttackState
    {
        public PlayerAttackState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
        {
            _enemyTroopSide = Faction.Allies.GetOpposite();
        }
    }
}
