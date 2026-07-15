using Assets.App.Scripts.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TroopModelManager : IEnemyTroopProvider, System.IDisposable
{
    private readonly RepositoryManager _repositoryManager;
    private readonly CoroutineStarter _coroutineStarter;

    private Coroutine _visionCoroutine = default;

    #region Disposable

    public void Dispose()
    {
        //StopperCoroutine(); // to do reload scene domains
    }

    #endregion

    public TroopModelManager(RepositoryManager repositoryManager, CoroutineStarter coroutineStarter)
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
        HashSet<EnemyTroopController> visibleEnemies = GetVisibleEnemies();
        List<TroopController> allEnemies = _repositoryManager.GetEnemyTroopControllersList();

        foreach (EnemyTroopController enemy in allEnemies)
        {
            if (visibleEnemies.Contains(enemy))
                enemy.TroopModelController.AppearTroopModel();

            else enemy.TroopModelController.DisappearTroopModel();
        }
    }

    #region Vision Logic

    private HashSet<EnemyTroopController> GetVisibleEnemies()
    {
        HashSet<EnemyTroopController> visibleEnemiesSet = new HashSet<EnemyTroopController>();
        List<TroopController> playerControllersList = _repositoryManager.GetPlayerTroopControllersList();

        foreach (PlayerTroopController playerController in playerControllersList)
        {
            TroopController[] enemiesInVisionRange = playerController.VisionController.GetEnemiesInVisionRange();

            foreach (EnemyTroopController unit in enemiesInVisionRange)
            {
                visibleEnemiesSet.Add(unit);
            }
        }

        return visibleEnemiesSet;
    }

    #endregion
}