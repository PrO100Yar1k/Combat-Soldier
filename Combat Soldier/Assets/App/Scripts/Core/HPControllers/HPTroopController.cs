using UnityEngine;
using App.Scripts.Core.Ability;
using App.Scripts.Core.Troops.TroopScripts;
using App.Scripts.Core.Canvases.ScreenCanvas;
using App.Scripts.Core.Troops.StateMachine.Defense_State;
using App.Scripts.Infrastructure.Enums;

namespace App.Scripts.Core.HPControllers
{
    public class HPTroopController : HPController // apply chain of responsibility pattern
    {
        protected readonly TroopScreenCanvasController _troopCanvasController;
        protected readonly TroopController _troopController;

        private int _currentDefensePoint;
        private float _currentBlockRate;

        public HPTroopController(TroopController troopController, TroopScreenCanvasController troopCanvasController)
        {
            _troopController = troopController;
            _troopCanvasController = troopCanvasController;

            InitializeData(_troopController.StatsController);
            UpdateSliderAndTextValues();
        }

        protected override void InitializeData(IStatsController statsController)
        {
            _unitName = _troopController.TroopScriptable.Name;

            _currentHealPoint = statsController.GetStatValueInt(StatType.MaxHealPoint);
            _currentDefensePoint = statsController.GetStatValueInt(StatType.MaxDefensePoint);
            _currentBlockRate = statsController.GetStatValueInt(StatType.BlockRate);
        }
        
        protected override void UpdateSliderAndTextValues()
        {
            _troopCanvasController.UpdateHealth(_currentHealPoint);
            _troopCanvasController.UpdateDefense(_currentDefensePoint);
        }

        #region Take Damage
    
        public override void TakeDamage(int attackDamage)
        {
            if (attackDamage <= 0)
                return;

            if (_troopController.StateController.CheckStateForActivity<TroopDefenseState>())
                TakeDamageWithDefenseState(attackDamage);
            else
                TakeDamageWithoutDefenseState(attackDamage);

            _troopController.TroopModelController.ChangeMaterialToDamaged();

            UpdateSliderAndTextValues();
            CheckHealPointsForDeath();
        }

        private void TakeDamageWithDefenseState(int attackDamage)
        {
            int blockedHP = Mathf.RoundToInt(attackDamage * _currentBlockRate);
            int takenDamage = attackDamage - blockedHP;

            if (_currentDefensePoint >= blockedHP) {
                _currentDefensePoint -= blockedHP;
            }
            else {
                _currentHealPoint -= blockedHP - _currentDefensePoint;
                _currentDefensePoint = 0;
            }

            _currentHealPoint -= takenDamage;
        }

        private void TakeDamageWithoutDefenseState(int attackDamage)
        {
            _currentHealPoint -= attackDamage;
        }

        #endregion

        #region Death

        protected override void HandleDeath()
        {
            if (_troopController == null)
                return;

            _troopController.StateController.ActivateDeathState();

            _troopController.Dispose();
            _troopController.StopAllCoroutines();

            UnityEngine.Object.Destroy(_troopController.gameObject);
            Debug.Log($"The {_unitName} was died");
        }

        #endregion
    }
}