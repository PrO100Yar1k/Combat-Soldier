using App.Scripts.Core.Scriptable;
using App.Scripts.Core.Services;
using UnityEngine;

namespace App.Scripts.Core.Troops.Troop_Scripts
{
    public class TroopVisionController
    {
        private readonly TroopController _troopController;
        private readonly TroopScriptable _troopScriptable;
        private readonly TargetSearchService _targetSearchService;

        public TroopVisionController(TroopController troopController, TroopScriptable troopScriptable, TargetSearchService targetSearchService)
        {
            _troopController = troopController;
            _troopScriptable = troopScriptable;

            _targetSearchService = targetSearchService;
        }

        public TroopController[] GetEnemiesInVisionRange()
        {
            Faction enemyTroopSide = GetEnemyTroopSide();

            float viewRange = _troopScriptable.ViewRangeRadius;
            Vector3 currentPosition = _troopController.transform.position;

            return _targetSearchService.GetEnemyListInRange(currentPosition, viewRange, enemyTroopSide);
        }

        private Faction GetEnemyTroopSide()
        {
            return _troopScriptable.TroopSide == Faction.Allies ? Faction.Enemies : Faction.Allies;
        }
    }
}
