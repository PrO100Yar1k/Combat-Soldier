using System.Collections;
using System.Collections.Generic;
using App.Scripts.Core.Troops.Troop_Instance;
using App.Scripts.Core.Troops.Troop_Scripts;
using App.Scripts.Infrastructure.Interfaces;
using App.Scripts.Repositories;
using UnityEngine;

namespace App.Scripts.Core.Troops.Model
{
    public class EnemyModelManager : IEnemyTroopProvider
    {
        private readonly TroopRepository _troopRepository;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly HashSet<TroopController> _visibleEnemiesSet
            = new HashSet<TroopController>();

        private readonly WaitForSeconds _checkDelay
            = new WaitForSeconds(_checkTroopDeploymentDelay);

        private const float _checkTroopDeploymentDelay = 0.3f;

        private Coroutine _visionCoroutine = default;

        public EnemyModelManager(TroopRepository troopRepository, ICoroutineRunner coroutineRunner)
        {
            _troopRepository = troopRepository;
            _coroutineRunner = coroutineRunner;
        }

        #region Coroutine Starter & Stopper

        public void StartProvidingEnemyDeploymentVision()
        {
            StopProvidingEnemyDeploymentVision();
            StartProvidingEnemyDeployment();
        }

        public void StopProvidingEnemyDeploymentVision()
        {
            if (_visionCoroutine == null)
                return;

            _coroutineRunner.StopCoroutine(_visionCoroutine);
            _visionCoroutine = null;
        }

        private void StartProvidingEnemyDeployment()
        {
            _visionCoroutine = _coroutineRunner.StartCoroutine(ProvideTroopDeploymentData());
        }

        #endregion

        private IEnumerator ProvideTroopDeploymentData()
        {
            while (true)
            {
                UpdateTroopDeploymentData();
                yield return _checkDelay;
            }
        }

        private void UpdateTroopDeploymentData()
        {
            var visibleEnemies = GetVisibleEnemies();
            var allEnemies = _troopRepository.GetEnemyTroops();

            foreach (TroopController enemy in allEnemies)
            {
                if (enemy.TroopModelController is IVisableModel visable)
                {
                    if (visibleEnemies.Contains(enemy))
                        visable.AppearTroopModel();
                    else
                        visable.DisappearTroopModel();
                }
            }
        }

        #region Vision Logic

        private HashSet<TroopController> GetVisibleEnemies()
        {
            _visibleEnemiesSet.Clear();

            var playerControllersList = _troopRepository.GetPlayerTroops();

            foreach (PlayerTroopController playerController in playerControllersList)
            {
                if (playerController == null || playerController.VisionController == null)
                    continue;

                TroopController[] enemiesInVisionRange = playerController.VisionController.GetEnemiesInVisionRange();

                if (enemiesInVisionRange == null)
                    continue;

                foreach (TroopController unit in enemiesInVisionRange)
                {
                    if (unit != null) 
                        _visibleEnemiesSet.Add(unit);
                }
            }

            return _visibleEnemiesSet;
        }

        #endregion
    }
}