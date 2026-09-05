using System;
using UnityEngine;
using System.Collections;
using App.Scripts.Core.ObjectPool;
using Assets.App.Scripts;
using Assets.App.Scripts.Core.Canvases;

public abstract class TroopDefenseState : TroopBaseState
{
    protected event Action<IDamagable, Vector3> OnActivateDefenseUnderAttack = default;

    protected Coroutine _defenseCoroutine = default;
    protected Coroutine _autoChangeStateCoroutine = default;

    protected const float _waitingForAutoChangeStateTime = 10f;
    protected const float _reactionTime = 0.5f;

    protected override string StateIconLocation
        => "State Icons/Defense-State-Icon";

    protected TroopDefenseState(TargetSearchService targetSearchService, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(targetSearchService, troopController, screenCanvasController, switcherState, animatorController)
    {

    }

    #region Events

    protected override void SubscribeToEvents()
    {
        OnActivateDefenseUnderAttack += FightBackToEnemy;
    }

    protected override void UnSubscribeFromEvents()
    {
        OnActivateDefenseUnderAttack -= FightBackToEnemy;
    }

    #endregion

    public override void OnStart()
    {
        PlayStateAnimation();
        StartAutoChangeState();
    }

    public override void OnStop()
    {
        StopAutoChangeState();
        StopFightBackCoroutine();
    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayDefense();
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        OnActivateDefenseUnderAttack?.Invoke(enemyDamagable, enemyPosition);
    }

    private void FightBackToEnemy(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        if (enemyDamagable == null)
            return;

        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        StartFightBackCoroutine(enemyDamagable, enemyPosition);
    }

    #region Fight Back Coroutine

    private void StartFightBackCoroutine(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        StopFightBackCoroutine();

        _defenseCoroutine = _troopController.StartCoroutine(FightBackCoroutine(enemyDamagable, enemyPosition));
    }

    private void StopFightBackCoroutine()
    {
        if (_defenseCoroutine == null)
            return;

        _troopController.StopCoroutine(_defenseCoroutine);
        _defenseCoroutine = null;
    }

    private IEnumerator FightBackCoroutine(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(_reactionTime);

        MonoBehaviour enemyMono = enemyDamagable as MonoBehaviour;

        if (enemyMono == null)
        {
            _switcherState.SwitchState<TroopDefaultState>();
            yield break;
        }

        Vector3 targetLookAtPosition = new Vector3(enemyMono.transform.position.x, _troopController.transform.position.y, enemyMono.transform.position.z);
        _troopController.transform.LookAt(targetLookAtPosition);

        BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
        bulletController.InitializeBullet(_troopController.transform.position, enemyPosition);

        yield return new WaitForSeconds(bulletController.GetBulletLifetime());

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyDamagable.TakeDamage(damageUnderAttack);

        if (enemyMono == null)
        {
            _switcherState.SwitchState<TroopDefaultState>();
            yield break;
        }

        string enemyName = (enemyDamagable as MonoBehaviour).name;
        Debug.Log($"I fought back to {enemyName} with total damage of {damageUnderAttack}!");

        StartAutoChangeState();
    }

    #endregion

    #region Helper Methods

    private bool isEnemyAlreadyDied(IDamagable enemyIDamagable)
        => (enemyIDamagable as MonoBehaviour) == null;

    #endregion

    #region Auto Change State Coroutine

    private void StartAutoChangeState()
    {
        StopAutoChangeState();

        _autoChangeStateCoroutine = _troopController.StartCoroutine(UpdatingStateCoroutine());
    }

    private void StopAutoChangeState()
    {
        if (_autoChangeStateCoroutine == null)
            return;

        _troopController.StopCoroutine(_autoChangeStateCoroutine);
        _autoChangeStateCoroutine = null;
    }

    private IEnumerator UpdatingStateCoroutine()
    {
        yield return new WaitForSeconds(_waitingForAutoChangeStateTime);
        _switcherState.SwitchState<TroopDefaultState>();
    }

    #endregion
}

public interface IReactableForDamage
{
    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable;
}