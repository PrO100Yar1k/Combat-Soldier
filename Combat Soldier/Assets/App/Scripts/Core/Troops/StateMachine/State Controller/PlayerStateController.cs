using System;
using System.Collections.Generic;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.StateMachine.Attack_State;
using App.Scripts.Core.Troops.StateMachine.Base;
using App.Scripts.Core.Troops.StateMachine.Death_State;
using App.Scripts.Core.Troops.StateMachine.Default_State;
using App.Scripts.Core.Troops.StateMachine.Defense_State;
using App.Scripts.Core.Troops.StateMachine.Move_State;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Troops.StateMachine.State_Controller
{
    public class PlayerStateController : TroopStateController
    {
        public PlayerStateController(TargetSearchService targetSearchService, TroopController troopController, ITroopAnimator animationController)
        {
            _states = new Dictionary<Type, TroopBaseState>
            {
                { typeof(TroopDefaultState), new PlayerDefaultState(targetSearchService, troopController, this, animationController) },
                { typeof(TroopDefenseState), new PlayerDefenseState(targetSearchService, troopController, this, animationController) },
                { typeof(TroopAttackState),  new PlayerAttackState(targetSearchService, troopController, this, animationController) },
                { typeof(TroopMoveState),    new PlayerMoveState(targetSearchService, troopController, this, animationController) },
                { typeof(TroopDeathState),   new PlayerDeathState(targetSearchService, troopController, this, animationController) }
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
