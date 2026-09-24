using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.State_Controller;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Infrastructure.Others;

namespace App.Scripts.Core.Troops.StateMachine.Attack_State
{
    public class PlayerAttackState : TroopAttackState
    {
        public PlayerAttackState(TargetSearchService targetSearchService, TroopController troopController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, switcherState, animatorController)
        {
            _enemyTroopSide = Faction.Allies.GetOpposite();
        }
    }
}
