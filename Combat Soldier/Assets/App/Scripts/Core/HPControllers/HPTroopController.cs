using UnityEngine;

public class HPTroopController : HPController
{
    protected readonly TroopScreenCanvasController _troopCanvasController = default;
    protected readonly TroopController _troopController = default;

    private int _currentDefensePoint = default;
    private float _currentBlockRate = default;

    public HPTroopController(TroopController troopController, TroopScreenCanvasController troopCanvasController, TroopScriptable troopScriptable)
    {
        _troopCanvasController = troopCanvasController;
        _troopController = troopController;

        AssignBasicParameters(troopScriptable); // think about updating values
        ChangeSliderAndTextValues();
    }

    protected override void AssignBasicParameters<T>(T scriptableObject)
    {
        TroopScriptable troopScriptable = scriptableObject as TroopScriptable;

        HPControllerName = troopScriptable.Name;
        _currentBlockRate = troopScriptable.BlockRate;

        _currentHealPoint = troopScriptable.MaxHealPoint;
        _currentDefensePoint = troopScriptable.MaxDefencePoint;
    }

    protected override void ChangeSliderAndTextValues()
    {
        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
        _troopCanvasController.ChangeDefensePointSlider(_currentDefensePoint);
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

        (_troopController as EnemyTroopController)?.TroopModelController.ChangeMaterialToDamaged(); // to do

        ChangeSliderAndTextValues();
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

    #region Defense Points

    public void IncreaseDefensePoints(int defensePoint)
    {
        if (defensePoint <= 0)
            return;

        _currentDefensePoint += defensePoint;

        ChangeSliderAndTextValues();
    }

    #endregion

    #region Death

    protected override void CheckHealPointsForDeath()
    {
        if (_troopController == null)
            return;

        if (_currentHealPoint <= 0) {
            TroopDeath(_troopController, _troopController.gameObject);
        }
    }

    protected override void TroopDeath<T>(T controller, GameObject objectToDestroy)
    {
        _troopController.StateController.ActivateDeathState();
        base.TroopDeath(controller, objectToDestroy);
    }

    #endregion
}