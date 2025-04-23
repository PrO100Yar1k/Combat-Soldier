using UnityEngine;

public class HPController
{
    private readonly TroopCanvasController _troopCanvasController;

    private string _currentName = default;

    private int _currentHealPoint = default;
    private int _currentDefensePoint = default;

    private float _currentBlockRate = default;

    public HPController(TroopScriptable currentDivisionScriptable, TroopCanvasController troopCanvasController)
    {
        _troopCanvasController = troopCanvasController;

        AssignBasicParameters(currentDivisionScriptable);
    }

    private void AssignBasicParameters(TroopScriptable currentDivisionScriptable)
    {
        _currentName = currentDivisionScriptable.Name;

        _currentHealPoint = currentDivisionScriptable.maxHealPoint;
        _currentDefensePoint = currentDivisionScriptable.maxDefencePoint;

        _currentBlockRate = currentDivisionScriptable.BlockRate;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        int blockedHP = (int) (damage * _currentBlockRate);
        int takenDamage = damage - blockedHP;

        if (_currentDefensePoint >= blockedHP) { // to do ?
            _currentDefensePoint -= blockedHP;
        }
        else {
            _currentHealPoint -= blockedHP - _currentDefensePoint;
            _currentDefensePoint = 0;
        }

        _currentHealPoint -= takenDamage;

        ChangeSliderAndTextValues();

        CheckTroopDeath();
    }

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

    private void ChangeSliderAndTextValues()
    {
        if (_troopCanvasController == null) //maybe not neccesary
            return;

        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
        _troopCanvasController.ChangeDefensePointSlider(_currentDefensePoint);
    }

    private void CheckTroopDeath() // to do
    {
        if (_currentHealPoint <= 0)
            TroopDeath();
    }

    private void TroopDeath()
    {
        Debug.Log($"The {_currentName} was died");

        // Destroy object
    }
}