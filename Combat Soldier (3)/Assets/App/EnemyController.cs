using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : TroopController
{
    [SerializeField] private TroopScriptable _currentDivision = default;

    [SerializeField] private bool isAttack = false;

    private TroopController currentEnemy = default;

    private Vector3 LastEnemyPosition = default;

    private void Awake()
    {

    }

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
    /*
    public void SetAttackPoint(Vector3 point)
    {
        isRunning = true;

        Vector3 currentPos = transform.position;

        //Vector3 offset = new Vector3(-2 * transform.forward.x, 0, -2 * transform.forward.z);
        Vector3 PointPos = new Vector3(point.x, currentPos.y, point.z);
        Vector3 offset = (PointPos - currentPos).normalized;

        Vector3 FinalPos = new Vector3(PointPos.x - offset.x, currentPos.y, PointPos.z - offset.z);

        transform.LookAt(FinalPos);

        float Distance = Vector3.Distance(FinalPos, currentPos);

        float timeToArrive = Distance / _currentDivision.speed;

        transform.DOMove(FinalPos, timeToArrive)
            .SetEase(Ease.Flash)
            .OnComplete(delegate {
                TryToAttackEnemy();
                isRunning = false;
            });
    } */

    private void TryToAttackEnemy()
    {
        //if (!isCanAttackEnemy())
        //    return;

        //StartCoroutine(AttackEnemy());
    }

    private IEnumerator AttackEnemy()
    {
        isAttack = true;

        //TroopController currentEnemy = EnemyInAttackRange();

        int currentcountAttackWaves = _currentDivision.countAttackWaves;

        float timeBetweenAttack = _currentDivision.timeToNextAttack;

        //while (isAttack)
        //{
            //if (!isCanAttackEnemy()) 
            //    yield return new WaitForSeconds(_currentDivision.timeViewAfterDetect);
            //    isAttack = false;
        //}

        while (currentcountAttackWaves > 0)
        {
            //currentEnemy.TakeDamage(_currentDivision.attackDamage);

            currentcountAttackWaves--;

            Debug.Log($"Attacked with damage {_currentDivision.attackDamage} ");

            yield return new WaitForSeconds(timeBetweenAttack);
        }

        isAttack = false;
    }

    // isCanAttackEnemy EnemyInViewRange EnemyInAttackRange 
}
