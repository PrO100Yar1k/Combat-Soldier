using System.Collections;
using App.Scripts.Core.Buildings.Base;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Services;
using App.Scripts.Core.Troops.State_Machine.State_Controller;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Troops.State_Machine.Default_State
{
    public class PlayerDefaultState : TroopDefaultState
    {
        public PlayerDefaultState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
            : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
        {

        }

        public override void OnStart()
        {
            PlayStateAnimation();
            CheckEnemyInAttackRange();
        }

        public override void OnStop()
        {

        }

        private void CheckEnemyInAttackRange()
        {
            _troopController.StartCoroutine(CheckEnemyOnceInAttackRange());
        }

        private IEnumerator CheckEnemyOnceInAttackRange(Faction targetFaction = Faction.Enemies, IDamagable targetPriorityEnemy = null)
        {
            const float initialDelay = 0.25f;

            yield return new WaitForSeconds(initialDelay);

            Vector3 currentPosition = _troopController.transform.position;
            float attackRange = _troopScriptable.AttackRangeRadius;

            MonoBehaviour enemyInAttackRange = _targetSearchService.GetClosestEnemyInRange(currentPosition, attackRange, targetFaction, targetPriorityEnemy, true);

            if (enemyInAttackRange == null)
                yield break;

            Vector3 targetLookAtPosition = new Vector3(enemyInAttackRange.transform.position.x, _troopController.transform.position.y, enemyInAttackRange.transform.position.z);
            _troopController.transform.LookAt(targetLookAtPosition);

            _troopController.StateController.ActivateAttackState(enemyInAttackRange as IDamagable);
        }
    }
}
