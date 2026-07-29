using Assets.App.Scripts.Repositories;
using System.Collections;
using UnityEngine;

namespace Assets.App.Scripts.Managers
{
    public class BuildingTargetManager
    {
        private readonly BuildingRepository _buildingRepository;
        private readonly ICoroutineRunner _coroutineRunner;

        private Coroutine _searchCoroutine = default;

        private const float _searchInterval = 0.5f;

        public BuildingTargetManager(BuildingRepository buildingRepository, ICoroutineRunner coroutineRunner)
        {
            _buildingRepository = buildingRepository;
            _coroutineRunner = coroutineRunner;
        }

        public void Initialize()
        {
            StopTargetSearching();
            StartTargetSearching();
        }

        private void StartTargetSearching()
        {
            _searchCoroutine = _coroutineRunner.StartCoroutine(ProcessTargetSearch());
        }

        public void StopTargetSearching()
        {
            if (_searchCoroutine == null)
                return;

            _coroutineRunner.StopCoroutine(_searchCoroutine);
        }

        public IEnumerator ProcessTargetSearch(Faction targetFaction = Faction.Allies, IDamagable targetPriorityEnemy = null)
        {
            WaitForSeconds waitingTime = new WaitForSeconds(_searchInterval);

            while (true)
            {
                var enemyBuildingList = _buildingRepository.GetEnemyBuildings();

                foreach (BuildingController buildingController in enemyBuildingList)
                {
                    buildingController.TryExecuteAttack();
                }

                yield return waitingTime;
            }
        }
    }
}
