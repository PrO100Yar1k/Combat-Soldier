using Assets.App.Scripts;
using Assets.App.Scripts.Infrastructure.Interfaces;
using Assets.App.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelManager : IEnemyTroopProvider, System.IDisposable
{
    private readonly TroopRepository _troopRepository;
    private readonly ICoroutineRunner _coroutineRunner;

    private Coroutine _visionCoroutine = default;

    #region Disposable

    public void Dispose()
    {
        //StopperCoroutine();
    }

    #endregion

    public EnemyModelManager(TroopRepository troopRepository, ICoroutineRunner coroutineRunner)
    {
        _troopRepository = troopRepository;
        _coroutineRunner = coroutineRunner;
    }

    #region Coroutine Starter & Stopper

    public void ProvideEnemyVisionStarter()
    {
        StopperCoroutine();
        StarterCoroutine();
    }

    private void StopperCoroutine()
    {
        if (_visionCoroutine == null)
            return;

        _coroutineRunner.StopCoroutine(_visionCoroutine);
        _visionCoroutine = null;
    }

    private void StarterCoroutine()
    {
        _visionCoroutine = _coroutineRunner.StartCoroutine(ProvideTroopDeploymentData());
    }

    #endregion

    private IEnumerator ProvideTroopDeploymentData()
    {
        while (true)
        {
            const float checkTroopDeploymentDelay = 0.3f;

            UpdateTroopDeploymentData();

            yield return new WaitForSeconds(checkTroopDeploymentDelay);
        }
    }

    private void UpdateTroopDeploymentData()
    {
        var visibleEnemies = GetVisibleEnemies();
        var allEnemies = _troopRepository.GetEnemyTroops();

        foreach (TroopController enemy in allEnemies)
        {
            if (enemy == null) continue;

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
        var visibleEnemiesSet = new HashSet<TroopController>();
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
                {
                    visibleEnemiesSet.Add(unit);
                }
            }
        }

        return visibleEnemiesSet;
    }

    #endregion
}