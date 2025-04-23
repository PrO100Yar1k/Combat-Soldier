using UnityEngine;

public class HPController
{
    private string currentName = default; // slider

    private int currentHealPoint = default;
    private int currentDefencePoint = default;

    private float currentBlockRate = default;

    public HPController(TroopScriptable currentDivisionScriptable)
    {
        currentName = currentDivisionScriptable.Name;

        currentHealPoint = currentDivisionScriptable.maxHealPoint;
        currentDefencePoint = currentDivisionScriptable.maxDefencePoint;

        currentBlockRate = currentDivisionScriptable.BlockRate;
    }

    public void TakeDamage(int Damage)
    {
        if (Damage <= 0)
            return;

        int blockedHP = (int) (Damage * currentBlockRate);
        int takenDamage = Damage - blockedHP;

        if (currentDefencePoint >= blockedHP) { // to do ?
            currentDefencePoint -= blockedHP;
        }
        else {
            currentHealPoint -= blockedHP - currentDefencePoint;
            currentDefencePoint = 0;
        }

        currentHealPoint -= takenDamage;

        CheckTroopDeath();
    }

    private void CheckTroopDeath() // to do
    {
        if (currentHealPoint <= 0)
            TroopDeath();
    }

    private void TroopDeath()
    {
        Debug.Log($"The {currentName} was died");

        // Destroy object
    }
}
