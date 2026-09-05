using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Enums;
using UnityEngine;

namespace App.Scripts.Core.Scriptable
{
    [CreateAssetMenu(fileName = "TroopScriptable", menuName = "Scriptable Objects/TroopScriptable")]
    public class TroopScriptable : UnityEngine.ScriptableObject
    {
        [Header("Basic Parameters")]

        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public AttackType AttackType { get; private set; }
        [field: SerializeField] public Faction TroopSide { get; private set; }

        [field: Space(8)]

        [Header("Heal & Defense Parameters")]

        [field: SerializeField] public int MaxHealPoint { get; private set; }
        [field: SerializeField] public int MaxDefencePoint { get; private set; }

        [field: SerializeField, Range(0, 1)] public float BlockRate { get; private set; }

        [field: Space(8)]

        [Header("Attack Parameters")]

        [field: SerializeField] public float TimeToReloadAttack { get; private set; }
        [field: SerializeField] public float TimeBetweenAttackWaves { get; private set; }

        [field: SerializeField] public int AttackDamage { get; private set; }
        [field: SerializeField] public int DamageUnderAttack { get; private set; }

        [field: SerializeField] public float AttackRangeRadius { get; private set; }
        [field: SerializeField] public int CountAttackWaves { get; private set; }

        [field: Space(8)]

        [Header("Others Parameters")]

        [field: SerializeField] public float ViewRangeRadius { get; private set; }

        [field: SerializeField] public float Speed { get; private set; }
    }
}
