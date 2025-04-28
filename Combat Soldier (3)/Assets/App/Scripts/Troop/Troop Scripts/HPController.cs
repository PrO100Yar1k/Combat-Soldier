using UnityEngine;

public class HPController
{
    private readonly ScreenCanvasController _troopCanvasController = default;
    private TroopController _troopController = default;

    private string _currentName = default;

    private int _currentHealPoint = default;
    private int _currentDefensePoint = default;

    private float _currentBlockRate = default;

    public HPController(TroopController troopController, ScreenCanvasController troopCanvasController, TroopScriptable troopScriptable)
    {
        _troopController = troopController;
        _troopCanvasController = troopCanvasController;

        AssignBasicParameters(troopScriptable);
        ChangeSliderAndTextValues();
    }

    private void AssignBasicParameters(TroopScriptable currentDivisionScriptable)
    {
        _currentName = currentDivisionScriptable.Name;

        _currentHealPoint = currentDivisionScriptable.MaxHealPoint;
        _currentDefensePoint = currentDivisionScriptable.MaxDefencePoint;

        _currentBlockRate = currentDivisionScriptable.BlockRate;
    }

    private void ChangeSliderAndTextValues()
    {
        if (_troopCanvasController == null) //maybe it is not neccesary
            return;

        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
        _troopCanvasController.ChangeDefensePointSlider(_currentDefensePoint);
    }

    #region Take Damage

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        if (_troopController.StateController.CheckStateForActivity<TroopDefenseState>())
            TakeDamageWithDefenseState(damage);
        
        else TakeDamageWithoutDefenseState(damage);

        ChangeSliderAndTextValues();

        CheckForTroopDeath();
    }

    private void TakeDamageWithDefenseState(int damage)
    {
        int blockedHP = (int)(damage * _currentBlockRate);
        int takenDamage = damage - blockedHP;

        if (_currentDefensePoint >= blockedHP) {
            _currentDefensePoint -= blockedHP;
        }
        else {
            _currentHealPoint -= blockedHP - _currentDefensePoint;
            _currentDefensePoint = 0;
        }

        _currentHealPoint -= takenDamage;
    }

    private void TakeDamageWithoutDefenseState(int damage)
    {
        _currentHealPoint -= damage;
    }

    #endregion

    #region Increase Points

    public void IncreaseHealPoints(int healPoint) // to think about namespacing
    {
        if (healPoint <= 0)
            return;

        _currentHealPoint += healPoint;

        ChangeSliderAndTextValues();
    }

    public void IncreaseDefensePoints(int defensePoint) // to think about namespacing
    {
        if (defensePoint <= 0)
            return;

        _currentDefensePoint += defensePoint;

        ChangeSliderAndTextValues();
    }

    #endregion

    #region Death

    private void CheckForTroopDeath() // to do
    {
        if (_currentHealPoint <= 0)
            TroopDeath();
    }

    private void TroopDeath()
    {
        Debug.Log($"The {_currentName} was died");

        _troopController.StopAllCoroutines();

        Object.DestroyImmediate(_troopController.gameObject);
    }

    #endregion
}