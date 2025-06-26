using System;
using System.Collections;
using UnityEngine;

public class TroopAttackState : TroopBaseState
{
    private event Action<IDamagable> OnActivateTroopAttack = default;

    private Coroutine _reloadAttackCoroutine = default;
    private Coroutine _attackCoroutine = default;

    private int _remainingAttackWaves = default;

    #region Events

    private void SubscribeToEvents()
    {
        OnActivateTroopAttack += TryToAttackEnemy;
    }

    private void UnSubscribeFromEvents()
    {
        OnActivateTroopAttack -= TryToAttackEnemy;
    }

    #endregion

    public TroopAttackState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState)
    {
        SetupDefaultCountAttackWaves();
    }

    public override void Start()
    {
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();

        DisableAttackCoroutine();
        ReloadAttackStarter();
    }

    public void ActivateTroopAttack(IDamagable enemyHPController)
        => OnActivateTroopAttack?.Invoke(enemyHPController);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/attack_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void TryToAttackEnemy(IDamagable enemyDamagable)
    {
        Vector3 troopPosition = _troopController.transform.position;
        TroopSide enemyTroopSide = TroopSide.Enemy;

        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyTroop = TroopGeneralManager.instance.GetClosestEnemyInRange(troopPosition, enemyTroopSide, attackRange, enemyDamagable);

        if (enemyTroop == null)
            return;

        if (enemyTroop.TryGetComponent(out TroopController troopController))
        {
            troopController.StateController.ActivateDefenceState(); 
            AttackEnemyCoroutineStarter(troopController);
        }
        else if (enemyTroop.TryGetComponent(out BuildingController buildingController))
        {
            AttackEnemyCoroutineStarter(buildingController);
        }
        
    }

    #region Coroutine Starter

    private void AttackEnemyCoroutineStarter<T>(T targetEnemy) where T : MonoBehaviour, IDamagable
    {
        DisableAttackCoroutine();

        EnableCoroutine(targetEnemy);
    }

    private void DisableAttackCoroutine()
    {
        if (_attackCoroutine == null)
            return;

        _troopController.StopCoroutine(_attackCoroutine);

        _attackCoroutine = null;
    }

    private void EnableCoroutine<T>(T enemyController) where T : MonoBehaviour, IDamagable
    {
        _attackCoroutine = _troopController.StartCoroutine(AttackEnemy(enemyController));
    }

    #endregion

    #region Attack Coroutine

    private IEnumerator AttackEnemy<T>(T enemyTroop) where T : MonoBehaviour, IDamagable
    {
        Debug.Log("Attacked");

        yield return new WaitUntil(()=> _remainingAttackWaves > 0);

        float timeBetweenAttackWaves = _troopScriptable.TimeBetweenAttackWaves;

        while (_remainingAttackWaves > 0)
        {
            if (enemyTroop == null)
                break;

            IResistable enemyResistable = enemyTroop as IResistable;

            enemyTroop.TakeDamage(_troopScriptable.AttackDamage);

            enemyResistable?.ActivateDefenseUnderAttack(_troopController.HPController);

            _remainingAttackWaves--;

            yield return new WaitForSeconds(timeBetweenAttackWaves);
        }

        ReloadAttackStarter();
    }

    #endregion

    #region Reload Attack

    private void ReloadAttackStarter()
    {
        if (_reloadAttackCoroutine != null)
            return;

        _reloadAttackCoroutine = _troopController.StartCoroutine(ReloadAttack());
    }

    private IEnumerator ReloadAttack()
    {
        float timeToReloadAttack = _troopScriptable.TimeToReloadAttack;

        yield return new WaitForSeconds(timeToReloadAttack);

        SetupDefaultCountAttackWaves();
    }

    private void SetupDefaultCountAttackWaves()
        => _remainingAttackWaves = _troopScriptable.CountAttackWaves;

    #endregion
}
