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
using UnityEngine;

namespace App.Scripts.Core.Troops.State_Machine.State_Controller
{
    public class EnemyStateController : TroopStateController
    {
        public EnemyStateController(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, Transform[] targetPointsList, ITroopAnimator animationController)
        {
            _states = new Dictionary<Type, TroopBaseState>
            {
                { typeof(TroopDefaultState), new EnemyDefaultState(targetSearchService, troopController, screenCanvasController, this, targetPointsList, animationController) },
                { typeof(TroopDefenseState), new EnemyDefenseState(targetSearchService, troopController, screenCanvasController, this, animationController) },
                { typeof(TroopAttackState),  new EnemyAttackState(targetSearchService, troopController, screenCanvasController, this, animationController) },
                { typeof(TroopMoveState),    new EnemyMoveState(targetSearchService, troopController, screenCanvasController, this, animationController) },

                { typeof(TroopDeathState),   new EnemyDeathState(targetSearchService, troopController, screenCanvasController, this, animationController) }
            };

            ActivateDefaultState();
        }
    }
}