using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace App.Scripts.Core.Services
{
    public class PatrolPointProvider
    {
        private readonly List<Transform> _patrolPoints;

        public PatrolPointProvider([Inject(Id = "Enemy Points")] List<Transform> patrolPoints)
        {
            _patrolPoints = new List<Transform>(patrolPoints);
        }

        public Transform[] GetRandomPatrolPoints()
        {
            return _patrolPoints
                .OrderBy(_ => Random.value)
                .ToArray();
        }
    }
}
