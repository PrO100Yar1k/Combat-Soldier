using UnityEngine;

[CreateAssetMenu(fileName = "TroopScriptable", menuName = "Scriptable Objects/TroopScriptable")]
public class TroopScriptable : ScriptableObject
{
    public string Name = default; // to add headers if it necessary

    public TroopType TroopType = default;
    public AttackType AttackType = AttackType.Land;
    public TroopSide TroopSide = TroopSide.Player;

    [Space(5)]

    public int MaxHealPoint = 100;
    public int MaxDefencePoint = 100;

    public float BlockRate = 0.2f;

    [Space(5)]

    public float TimeToReloadAttack = 5;
    public float TimeBetweenAttackWaves = 5;

    public int AttackDamage = 25;
    public float AttackRangeRadius = 3;

    public int CountAttackWaves = 5;

    [Space(5)]

    public float ViewRangeRadius = 10;

    public int TroopsPerUnit = 50;

    public float Speed = 2.5f;  // max speed; min speed; timeToMaxSpeed; SpeedUnderAttack ???
}