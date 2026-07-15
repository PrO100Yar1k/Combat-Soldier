using System;
using UnityEngine;
using System.Collections;
using Assets.App.Scripts;

public abstract class TroopDefenseState : TroopBaseState
{
    protected event Action<IDamagable, Vector3> OnActivateDefenseUnderAttack = default;
    protected Coroutine _updatingStateCoroutine = default;

    protected const float _waitingForAttackCooldownTime = 10f;
    protected const float _reactionTime = 0.5f;

    protected override string StateIconLocation
        => "State Icons/Defense-State-Icon";

    protected TroopDefenseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState, ITroopAnimator animatorController)
        : base(repositoryManager, troopController, screenCanvasController, switcherState, animatorController)
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
        UpdatingStateRestarter();
    }

    public override void OnStop()
    {
        // stop coroutine
    }

    protected override void PlayStateAnimation()
    {
        _animatorController.PlayDefense();
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyDamagable, Vector3 enemyPosition)
        => OnActivateDefenseUnderAttack?.Invoke(enemyDamagable, enemyPosition);

    private void FightBackToEnemy(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        if (enemyDamagable == null)
            return;

        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        _troopController.StartCoroutine(FightBackCoroutine(enemyDamagable, enemyPosition));
    }

    private IEnumerator FightBackCoroutine(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        string enemyName = (enemyDamagable as UnityEngine.Object).name;

        yield return new WaitForSeconds(_reactionTime);

        if (isEnemyAlreadyDied(enemyDamagable))
        {
            _switcherState.SwitchState<TroopDefaultState>();
            yield break;
        }

        BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
        bulletController.InitializeBullet(_troopController.transform.position, enemyPosition);

        yield return new WaitForSeconds(bulletController.GetBulletLifetime());

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyDamagable.TakeDamage(damageUnderAttack);

        if (isEnemyAlreadyDied(enemyDamagable))
        {
            _switcherState.SwitchState<TroopDefaultState>();
            yield break;
        }

        UpdatingStateRestarter();

        Debug.Log($"I fought back to {enemyName} with total damage of {damageUnderAttack}!");
    }

    #region Helper Methods

    private bool isEnemyAlreadyDied(IDamagable enemyIDamagable)
        => (enemyIDamagable as MonoBehaviour) == null;

    #endregion

    #region Updating State Coroutine

    private void UpdatingStateRestarter()
    {
        if (_updatingStateCoroutine != null)
        {
            _troopController.StopCoroutine(_updatingStateCoroutine);
            _updatingStateCoroutine = null;
        }

        _updatingStateCoroutine = _troopController.StartCoroutine(UpdatingStateCoroutine());
    }

    private IEnumerator UpdatingStateCoroutine()
    {
        yield return new WaitForSeconds(_waitingForAttackCooldownTime);
        _switcherState.SwitchState<TroopDefaultState>();
    }

    #endregion
}

public interface IReactableForDamage
{
    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable;
}