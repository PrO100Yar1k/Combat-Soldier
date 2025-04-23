using UnityEngine;

[CreateAssetMenu(fileName = "TroopScriptable", menuName = "Scriptable Objects/TroopScriptable")]
public class TroopScriptable : ScriptableObject
{
    public string Name = default; // to add headers

    public TroopType TroopType = default;
    public AttackType AttackType = AttackType.Land;

    public int currentHealPoint = default; // maybe float
    public int currentDefencePoint = default;

    [Space(5)]

    public int maxHealPoint = 100;

    [Space(5)]

    public int maxDefencePoint = 100;
    public float BlockRate = 0.2f;

    [Space(5)]

    public float timeToNextAttack = 5;

    public int countAttackWaves = 5;
    public int attackDamage = 25;

    public float attackRangeRadius = 3;

    [Space(5)]

    public float viewRangeRadius = 10;

    public int countInDivision = 50;

    public float speed = 2.5f;  // max speed; min speed; timeToMaxSpeed; SpeedUnderAttack ???
}