using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopModelManager : MonoBehaviour
{
    private Coroutine _visionCoroutine = default;

    #region Events

    //private void OnEnable()
    //    => SubscribeToEvents();

    //private void OnDisable()
    //    => UnSubscribeFromEvents();

    private void SubscribeToEvents()
    {
        GameEvents.instance.OnTroopStartedMovement += ProvideEnemyVisionCoroutineStarter;

        GameEvents.instance.OnTroopFinishedMovement += ProvideEnemyVisionCoroutineStopper;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopStartedMovement -= ProvideEnemyVisionCoroutineStarter;

        GameEvents.instance.OnTroopFinishedMovement -= ProvideEnemyVisionCoroutineStopper;
    }

    #endregion

    private void Start()
    {
        ProvideEnemyVisionCoroutineStarter();
    }

    #region Coroutine Starter & Stopper

    private void ProvideEnemyVisionCoroutineStarter()
    {
        if (_visionCoroutine != null)
            ProvideEnemyVisionCoroutineStopper();
        else
            _visionCoroutine = StartCoroutine(ProvideTroopDeploymentData());
    }

    private void ProvideEnemyVisionCoroutineStopper() // ?
    {
        if (_visionCoroutine == null)
            return;

        StopCoroutine(_visionCoroutine);
        _visionCoroutine = null;
    }

    #endregion

    private IEnumerator ProvideTroopDeploymentData()
    {
        while (true)
        {
            const float delayPerFrame = 0.3f;

            UpdateTroopDeploymentData();

            yield return new WaitForSeconds(delayPerFrame);
        }
    }

    private void UpdateTroopDeploymentData()
    {
        DisableAllEnemies();
        EnableAllVisibleEnemies();
    }

    #region Enable & Disable Enemies

    private void EnableAllVisibleEnemies()
    {
        EnemyTroopController[] enemyControllers = GetVisibleEnemies();

        foreach (EnemyTroopController enemyController in enemyControllers)
        {
            enemyController.TroopModelController.AppearTroopModel();
        }
    }

    private void DisableAllEnemies()
    {
        List<TroopController> enemyControllersList = new List<TroopController>(TroopGeneralManager.instance.GetEnemyTroopControllersList());

        foreach (EnemyTroopController troopController in enemyControllersList)
        {
            troopController.TroopModelController.DisappearTroopModel();
        }
    }

    private EnemyTroopController[] GetVisibleEnemies()
    {
        List<EnemyTroopController> targetList = new List<EnemyTroopController>();
        List<TroopController> playerControllersList = new List<TroopController>(TroopGeneralManager.instance.GetPlayerTroopControllersList());

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
