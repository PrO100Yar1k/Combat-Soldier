using System.Collections;
using UnityEngine;

public class TroopVisionController
{
    private TroopController _troopController = default;
    private TroopScriptable _troopScriptable = default;

    private TroopSide _troopSide = default;

    private Coroutine _visionCoroutine = default;

    #region Events

    private void SubscribeToEvents()
    {
        //GameEvents.instance.OnTroopStartedMovement += ProvideEnemyVisionCoroutineStarter;

        GameEvents.instance.OnTroopDied += DisableObject;
    }

    private void UnSubscribeFromEvents()
    {
        GameEvents.instance.OnTroopDied -= DisableObject;
    }

    #endregion

    public TroopVisionController(TroopController troopController, TroopScriptable troopScriptable, TroopSide troopSide)
    {
        _troopController = troopController;
        _troopScriptable = troopScriptable;

        _troopSide = troopSide;

        SubscribeToEvents();

        DisableAllEnemies();

        ProvideVisionCoroutineStarter(); // make an event with movement any troop and subscibe to it
    }

    private void ProvideVisionCoroutineStarter()
    {
        if (_visionCoroutine != null)
        {
            _troopController.StopCoroutine(_visionCoroutine);
            _visionCoroutine = null;
        }
        else
        {
            _visionCoroutine = _troopController.StartCoroutine(ProvideEnemyDeploymentData());
        }
    }

    private IEnumerator ProvideEnemyDeploymentData()
    {
        while (true)
        {
            Vector3 currentPosition = _troopController.transform.position;
            float viewRange = _troopScriptable.ViewRangeRadius;

            TroopSide enemyTroopSide = GetEnemyTroopSide();

            TroopController[] EnemyControllersList = TroopGeneralManager.instance.GetEnemyListInRange(currentPosition, viewRange, enemyTroopSide);

            if (EnemyControllersList.Length > 0)
            {

            }

            yield return new WaitForSeconds(1);
        }
    }

    private void DisableAllEnemies()
    {
        TroopController[] EnemyControllersList = TroopGeneralManager.instance.GetTroopControllersList(GetEnemyTroopSide()).ToArray();

        foreach (TroopController troopController in EnemyControllersList)
        {
            //troopController.GetComponent<MeshRenderer>().material = 
        }
    }

    private TroopSide GetEnemyTroopSide()
        => _troopSide == TroopSide.Player ? TroopSide.Enemy : TroopSide.Player;

    private void DisableObject(TroopController troopController, TroopSide troopSide)
    {
        UnSubscribeFromEvents();
    }
}
