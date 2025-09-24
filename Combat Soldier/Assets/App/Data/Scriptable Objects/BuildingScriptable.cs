using UnityEngine;

[CreateAssetMenu(fileName = "BuildingScriptable", menuName = "Scriptable Objects/BuildingScriptable")]
public class BuildingScriptable : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = default;
    [field: SerializeField] public int MaxHealPoint { get; private set; } = 100;

    [field: SerializeField] public int Damage { get; private set; } = 20;

    [field: SerializeField] public float AttackRange { get; private set; } = 5;
    [field: SerializeField] public int AttackWave { get; private set; } = 1;

    [field: SerializeField] public float ReloadingTime { get; private set; } = 3;
    [field: SerializeField] public float TimeBetweenWaves { get; private set; } = 0;
}
