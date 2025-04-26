using UnityEngine;

[CreateAssetMenu(fileName = "TroopScriptable", menuName = "Scriptable Objects/TroopScriptable")]
public class TroopScriptable : ScriptableObject
{
    public string Name = default; // to add headers if it necessary

    public TroopType TroopType = default;
    public AttackType AttackType = AttackType.Land;

    [Space(5)]

    public int maxHealPoint = 100;
    public int maxDefencePoint = 100;

    public float BlockRate = 0.2f;

    [Space(5)]

    public float timeToReloadAttack = 5;
    public float timeBetweenAttackWaves = 5;

    public int countAttackWaves = 5;
    public int attackDamage = 25;
    public float attackRangeRadius = 3;

    [Space(5)]

    public float viewRangeRadius = 10;

    public int troopsPerUnit = 50;

    public float speed = 2.5f;  // max speed; min speed; timeToMaxSpeed; SpeedUnderAttack ???
}