using UnityEngine;

namespace App.Scripts.Infrastructure.Others
{
    public static class CombatMath
    {
        private const float _defaultDistanceDelta = 0.1f;

        public static Vector3 GetAttackDestination(Vector3 currentPosition, Vector3 targetPosition, float attackRange, float delta = _defaultDistanceDelta)
        {
            Vector3 direction = (targetPosition - currentPosition).normalized;
            float adjustedDistance = attackRange * (1f - delta);

            return targetPosition - (direction * adjustedDistance);
        }
    }
}
