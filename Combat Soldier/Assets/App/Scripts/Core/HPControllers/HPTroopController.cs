using Assets.App.Scripts.Core.Canvases;
using Assets.App.Scripts.Core.Health;
using UnityEngine;

public class HPTroopController : HPController<TroopScriptable>
{
    protected readonly TroopScreenCanvasController _troopCanvasController = default;
    protected readonly TroopController _troopController = default;

    private int _currentDefensePoint = default;
    private float _currentBlockRate = default;

    public HPTroopController(TroopController troopController, TroopScreenCanvasController troopCanvasController, TroopScriptable troopScriptable) : base(troopScriptable)
    {
        _troopCanvasController = troopCanvasController;
        _troopController = troopController;

        UpdateSliderAndTextValues();
    }

    protected override void InitializeData(TroopScriptable troopScriptable)
    {
        _unitName = troopScriptable.Name;

        _currentHealPoint = troopScriptable.MaxHealPoint;
        _currentDefensePoint = troopScriptable.MaxDefencePoint;

        _currentBlockRate = troopScriptable.BlockRate;
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

        int blockedHP = (int) (attackDamage * _currentBlockRate);
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