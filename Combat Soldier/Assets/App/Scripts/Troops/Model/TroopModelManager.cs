using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TroopModelManager : System.IDisposable
{
    private readonly RepositoryManager _repositoryManager;
    private readonly CoroutineStarter _coroutineStarter;

    private Coroutine _visionCoroutine = default;

    #region Disposable

    public void Dispose()
    {
        StopperCoroutine();
    }

    #endregion

    public TroopModelManager(RepositoryManager repositoryManager, CoroutineStarter coroutineStarter)
    {
        _repositoryManager = repositoryManager;
        _coroutineStarter = coroutineStarter;

        ProvideEnemyVisionCoroutineStarter();
    }

    #region Coroutine Starter & Stopper

    private void ProvideEnemyVisionCoroutineStarter()
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
        DisableAllEnemies();
        EnableAllVisibleEnemies();
    }

    #region Enable & Disable Enemies

    private void DisableAllEnemies()
    {
        List<TroopController> enemyControllersList = new List<TroopController>(_repositoryManager.GetEnemyTroopControllersList());

        foreach (EnemyTroopController troopController in enemyControllersList)
        {
            troopController.TroopModelController.DisappearTroopModel();
        }
    }

    private void EnableAllVisibleEnemies()
    {
        EnemyTroopController[] enemyControllers = GetVisibleEnemies();

        foreach (EnemyTroopController enemyController in enemyControllers)
        {
            enemyController.TroopModelController.AppearTroopModel();
        }
    }

    private EnemyTroopController[] GetVisibleEnemies()
    {
        List<EnemyTroopController> targetList = new List<EnemyTroopController>();
        List<TroopController> playerControllersList = new List<TroopController>(_repositoryManager.GetPlayerTroopControllersList());

        foreach (PlayerTroopController playerController in playerControllersList)
        {
            TroopController[] playerControllersInVisionRange = playerController.VisionController.GetEnemiesInVisionRange();

            foreach (EnemyTroopController unit in playerControllersInVisionRange)
            {
                targetList.Add(unit);
            }
        }

        return targetList.ToArray();
    }

    #endregion
}
