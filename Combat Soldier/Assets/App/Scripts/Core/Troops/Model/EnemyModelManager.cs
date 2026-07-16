using Assets.App.Scripts.Infrastructure.Interfaces;
using System.Collections.Generic;
using Assets.App.Scripts;
using System.Collections;
using UnityEngine;

public class EnemyModelManager : IEnemyTroopProvider, System.IDisposable
{
    private readonly RepositoryManager _repositoryManager;
    private readonly CoroutineStarter _coroutineStarter;

    private Coroutine _visionCoroutine = default;

    #region Disposable

    public void Dispose()
    {
        //StopperCoroutine();
    }

    #endregion

    public EnemyModelManager(RepositoryManager repositoryManager, CoroutineStarter coroutineStarter)
    {
        _repositoryManager = repositoryManager;
        _coroutineStarter = coroutineStarter;
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

        _coroutineStarter.StopCoroutine(_visionCoroutine);
        _visionCoroutine = null;
    }

    private void StarterCoroutine()
    {
        _visionCoroutine = _coroutineStarter.StartCoroutine(ProvideTroopDeploymentData());
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
        HashSet<TroopController> visibleEnemies = GetVisibleEnemies();
        List<TroopController> allEnemies = _repositoryManager.GetEnemyTroopControllersList();

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
        HashSet<TroopController> visibleEnemiesSet = new HashSet<TroopController>();
        List<TroopController> playerControllersList = _repositoryManager.GetPlayerTroopControllersList();

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