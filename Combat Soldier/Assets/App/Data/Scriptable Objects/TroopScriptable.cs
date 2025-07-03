using UnityEngine;

[CreateAssetMenu(fileName = "TroopScriptable", menuName = "Scriptable Objects/TroopScriptable")]
public class TroopScriptable : ScriptableObject
{
    [Header("Basic Parameters")]
    [field: SerializeField] public string Name { get; private set; } = default;

    [field: SerializeField] public TroopType TroopType { get; private set; } = default;
    [field: SerializeField] public AttackType AttackType { get; private set; } = default;
    [field: SerializeField] public TroopSide TroopSide { get; private set; } = default;

    [field: Space(5)]

    [Header("Heal & Defense Parameters")]

    [field: SerializeField] public int MaxHealPoint { get; private set; } = 100;
    [field: SerializeField] public int MaxDefencePoint { get; private set; } = 100;

    [field: SerializeField, Range(0, 1)] public float BlockRate { get; private set; } = 0.2f;

    [field: Space(5)]

    [Header("Attack Parameters")]

    [field: SerializeField] public float TimeToReloadAttack { get; private set; } = 5;
    [field: SerializeField] public float TimeBetweenAttackWaves { get; private set; } = 5;

    [field: SerializeField] public int AttackDamage { get; private set; } = 25;
    [field: SerializeField] public int DamageUnderAttack { get; private set; } = 10;

    [field: SerializeField] public float AttackRangeRadius { get; private set; } = 3;
    [field: SerializeField] public int CountAttackWaves { get; private set; } = 5;

    [field: Space(5)]

    [Header("Others Parameters")]

    [field: SerializeField] public float ViewRangeRadius { get; private set; } = 7;

    [field: SerializeField] public int TroopsPerUnit { get; private set; } = 50;

    [field: SerializeField] public float Speed { get; private set; } = 7f;
    
    // maybe create additional parameters like: MaxSpeed; MinSpeed; TimeToGetMaxSpeed; SpeedUnderAttack
}