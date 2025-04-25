using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTroopController : MonoBehaviour //TroopController
{
    [SerializeField] private TroopScriptable _currentDivision = default;

    private TroopController currentEnemy = default;

    private Vector3 LastEnemyPosition = default;

    private void Awake()
    {

    }


    //protected override void InitializeTroop()
    //{

    //}

    private void FixedUpdate()
    {
        //if (isAttack || isRunning)
        //    return;

        //if (EnemyInViewRange())
        //{
            //SetAttackPoint(LastEnemyPosition);
        //}
    }

    private void LateUpdate()
    {
        //if (Vector3.Distance(transform.position, currentEnemy.transform.position) <= _currentDivision.viewRangeRadius)
        //{
        //    Vector3 PointToLookForEnemy = new Vector3(currentEnemy.transform.position.x, transform.position.y, currentEnemy.transform.position.z);
        //    transform.LookAt(PointToLookForEnemy);
        //}
    }



    // isCanAttackEnemy EnemyInViewRange EnemyInAttackRange 
}
