using UnityEngine;

[CreateAssetMenu(fileName = "HPScriptable", menuName = "Scriptable Objects/HPScriptable")]
public class HPScriptable : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = default;
    [field: SerializeField] public uint ID { get; private set; } = 9999;

    [field: SerializeField] public int MaxHealPoint { get; private set; } = 100;
}
