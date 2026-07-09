using System;
using UnityEngine;
using System.Collections;

public abstract class TroopDefenseState : TroopBaseState
{
    protected event Action<IDamagable, Vector3> OnActivateDefenseUnderAttack = default;

    protected Coroutine _updatingStateCoroutine = default;

    protected const float _waitingForAttackTime = 10f;

    protected const float _reactionTime = 0.5f;

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

    public TroopDefenseState(RepositoryManager repositoryManager, TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState)
        : base(repositoryManager, troopController, screenCanvasController, switcherState) 
    {
    
    }

    public override void Start()
    {
        SubscribeToEvents();
        UpdatingStateStarter();

        EnableStateIcon();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
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

        BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
        bulletController.InitializeBullet(_troopController.transform.position, enemyPosition);

        yield return new WaitForSeconds(bulletController.GetBulletLifetime());

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyDamagable.TakeDamage(damageUnderAttack);

        if (isEnemyAlreadyDied(enemyDamagable))
            _switcherState.SwitchState<TroopDefaultState>();

        UpdatingStateRestarter();

        Debug.Log($"I fought back to {enemyName} with total damage of {damageUnderAttack}!");
    }

    #region Helper Methods

    private bool isEnemyAlreadyDied(IDamagable enemyIDamagable)
        => (enemyIDamagable as MonoBehaviour) == null;

    #endregion

    #region Updating State Coroutine

    private void UpdatingStateStarter()
    {
        if (_updatingStateCoroutine != null)
            return;

        _updatingStateCoroutine = _troopController.StartCoroutine(UpdatingStateCoroutine());
    }

    private void UpdatingStateRestarter()
    {
        if (_updatingStateCoroutine == null)
            return;

        _troopController.StopCoroutine(_updatingStateCoroutine);
        _updatingStateCoroutine = null;

        UpdatingStateStarter();
    }

    private IEnumerator UpdatingStateCoroutine()
    {
        yield return new WaitForSeconds(_waitingForAttackTime);

        _switcherState.SwitchState<TroopDefaultState>();
    }

    #endregion
}

public interface IReactableForDamage
{
    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable;
}