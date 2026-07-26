using Assets.App.Scripts.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetSearchService
{
    private readonly TroopRepository _troopRepository;
    private readonly BuildingRepository _buildingRepository;

    public TargetSearchService(TroopRepository troopRepository, BuildingRepository buildingRepository)
    {
        _troopRepository = troopRepository;
        _buildingRepository = buildingRepository;
    }

    public TroopController[] GetEnemyListInRange(Vector3 troopPosition, float troopRange, Faction enemyTroopSide)
    {
        return _troopRepository.GetTroops(enemyTroopSide).ToArray()
            .Where(troop => Vector3.Distance(troopPosition, troop.transform.position) <= troopRange)
            .ToArray();
    }

    public MonoBehaviour GetClosestEnemyInRange(Vector3 origin, float range, Faction enemyFaction, IDamagable priorityTarget = null, bool includeBuildings = true)
    {
        float rangeSqr = range * range;
        float closestDistanceSqr = Mathf.Infinity;

        MonoBehaviour closestEnemy = null;

        var candidates = new List<MonoBehaviour>(_troopRepository.GetTroops(enemyFaction));

        if (includeBuildings)
        {
            candidates.AddRange(_buildingRepository.GetEnemyBuildings());
        }

        foreach (var enemy in candidates)
        {
            if (enemy == null)
                continue;

            float distSqr = (enemy.transform.position - origin).sqrMagnitude;

            if (distSqr > rangeSqr)
                continue;

            if (priorityTarget != null && enemy.TryGetComponent<IDamagable>(out var damagable) && damagable.Equals(priorityTarget))
                return enemy;

            if (distSqr < closestDistanceSqr && enemy.GetComponent<IDamagable>() != null)
            {
                closestDistanceSqr = distSqr;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}