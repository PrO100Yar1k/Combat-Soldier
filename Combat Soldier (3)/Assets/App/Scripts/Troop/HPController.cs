using UnityEngine;

public class HPController
{
    private TroopCanvasController _troopCanvasController;

    private string _currentName = default;

    private int _currentHealPoint = default;
    private int _currentDefencePoint = default;

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
        _currentDefencePoint = currentDivisionScriptable.maxDefencePoint;

        _currentBlockRate = currentDivisionScriptable.BlockRate;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        int blockedHP = (int) (damage * _currentBlockRate);
        int takenDamage = damage - blockedHP;

        if (_currentDefencePoint >= blockedHP) { // to do ?
            _currentDefencePoint -= blockedHP;
        }
        else {
            _currentHealPoint -= blockedHP - _currentDefencePoint;
            _currentDefencePoint = 0;
        }

        _currentHealPoint -= takenDamage;

        ChangeSliderValues();

        CheckTroopDeath();
    }

    private void ChangeSliderValues()
    {
        if (_troopCanvasController == null) //maybe not neccesary
            return;

        _troopCanvasController.ChangeHealPointSlider(_currentHealPoint);
        _troopCanvasController.ChangeDefensePointSlider(_currentDefencePoint);
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