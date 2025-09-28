using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TroopModelManager : MonoBehaviour
{
    private Coroutine _visionCoroutine = default;

    private void Start()
    {
        ProvideEnemyVisionCoroutineStarter();
    }

    #region Coroutine Starter & Stopper

    private void ProvideEnemyVisionCoroutineStarter()
    {
        CoroutineStopper();

        CoroutineStarter();
    }

    private void CoroutineStopper()
    {
        if (_visionCoroutine == null)
            return;

        StopCoroutine(_visionCoroutine);

        _visionCoroutine = null;
    }

    private void CoroutineStarter()
    {
        _visionCoroutine = StartCoroutine(ProvideTroopDeploymentData());
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
        List<TroopController> enemyControllersList = new List<TroopController>(RepositoryManager.instance.GetEnemyTroopControllersList());

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
        List<TroopController> playerControllersList = new List<TroopController>(RepositoryManager.instance.GetPlayerTroopControllersList());

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
