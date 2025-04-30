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
        //UpdateTroopDeploymentData(); // change it? not in start
        ProvideEnemyVisionCoroutineStarter();
    }

    private TroopController[] GetVisibleTroops()
    {
        List<TroopController> targetList = new List<TroopController>();
        List<TroopController> enemyControllersList = TroopGeneralManager.instance.GetTroopControllersList(TroopSide.Player); // ?

        foreach (TroopController playerController in enemyControllersList)
        {
            TroopController[] playerControllersInVisionRange = playerController.VisionController.GetEnemiesInVisionRange();

            foreach (TroopController unit in playerControllersInVisionRange)
            {
                targetList.Add(unit);
            }
        }

        return targetList.ToArray();
    }


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
        EnableAllVisibleTroops();
    }


    private void EnableAllVisibleTroops()
    {
        TroopController[] troopControllers = GetVisibleTroops(); // player side

        foreach (TroopController troopController in troopControllers)
        {
            troopController.TroopModelController.AppearTroopModel();
        }
    }

    private void DisableAllEnemies()
    {
        List<TroopController> list = new List<TroopController>(TroopGeneralManager.instance.GetTroopControllersList(TroopSide.Enemy));

        foreach (TroopController troopController in list)
        {
            troopController.TroopModelController.DisappearTroopModel();
        }
    }

}
