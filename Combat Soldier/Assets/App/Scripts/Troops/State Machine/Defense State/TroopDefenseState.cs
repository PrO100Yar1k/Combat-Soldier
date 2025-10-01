using System;
using UnityEngine;
using System.Collections;

public abstract class TroopDefenseState : TroopBaseState
{
    private event Action<IDamagable, Vector3> OnActivateDefenseUnderAttack = default;

    private const float _reactionTime = 0.5f;

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

    public TroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) 
    {
    
    }

    public override void Start()
    {
        SubscribeToEvents();
        EnableStateIcon();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyDamagable, Vector3 enemyPosition)
        => OnActivateDefenseUnderAttack?.Invoke(enemyDamagable, enemyPosition);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        if (_troopController == null || enemyDamagable == null)
            return;

        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        _troopController.StartCoroutine(FightBackCoroutine(enemyDamagable, enemyPosition));
    }

    private IEnumerator FightBackCoroutine(IDamagable enemyDamagable, Vector3 enemyPosition)
    {
        //

        string enemyName = (enemyDamagable as UnityEngine.Object).name;

        yield return new WaitForSeconds(_reactionTime);

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

        Debug.Log($"I fought back to {enemyName} with total damage of {damageUnderAttack}!");
    }

    private bool isEnemyAlreadyDied(IDamagable enemyIDamagable)
        => (enemyIDamagable as MonoBehaviour) == null;
}

public interface IReactableForDamage
{
    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable;
}