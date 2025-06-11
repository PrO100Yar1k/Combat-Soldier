using UnityEngine;

[CreateAssetMenu(fileName = "BuildingScriptable", menuName = "Scriptable Objects/BuildingScriptable")]
public class BuildingScriptable : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = default;
    [field: SerializeField] public int MaxHealPoint { get; private set; } = 100;

}
