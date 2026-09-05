using System;
using System.Collections.Generic;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.Attack_State;
using App.Scripts.Core.Troops.State_Machine.Base;
using App.Scripts.Core.Troops.State_Machine.Death_State;
using App.Scripts.Core.Troops.State_Machine.Default_State;
using App.Scripts.Core.Troops.State_Machine.Defense_State;
using App.Scripts.Core.Troops.State_Machine.Move_State;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.State_Machine.State_Controller
{
    public class PlayerStateController : TroopStateController
    {
        public PlayerStateController(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ITroopAnimator animationController)
        {
            _states = new Dictionary<Type, TroopBaseState>
            {
                { typeof(TroopDefaultState), new PlayerDefaultState(targetSearchService, troopController, screenCanvasController, this, animationController) },
                { typeof(TroopDefenseState), new PlayerDefenseState(targetSearchService, troopController, screenCanvasController, this, animationController) },
                { typeof(TroopAttackState),  new PlayerAttackState(targetSearchService, troopController, screenCanvasController, this, animationController) },
                { typeof(TroopMoveState),    new PlayerMoveState(targetSearchService, troopController, screenCanvasController, this, animationController) },

                { typeof(TroopDeathState),   new PlayerDeathState(targetSearchService, troopController, screenCanvasController, this, animationController) }
            };

            ActivateDefaultState();
        }

        public bool TrySwitchToOppositeState() //maybe remove this feature
        {
            bool isAttack = CheckStateForActivity<TroopAttackState>();
            bool isDefense = CheckStateForActivity<TroopDefenseState>();

            if (!isAttack && !isDefense)
                return false;

            if (isAttack) SwitchState<TroopDefenseState>();
            else if (isDefense) SwitchState<TroopAttackState>();

            return true;
        }
    }
}
