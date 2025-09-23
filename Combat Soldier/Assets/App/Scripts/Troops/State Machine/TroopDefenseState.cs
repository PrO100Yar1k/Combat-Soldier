using System;
using System.Collections;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    private event Action<HPController, Vector3> OnActivateDefenseUnderAttack = default;

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

    public TroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableTroopState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    public void ActivateDefenseUnderAttack(HPController enemyHPController, Vector3 enemyPosition)
        => OnActivateDefenseUnderAttack?.Invoke(enemyHPController, enemyPosition);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(HPController enemyHPController, Vector3 enemyPosition) // make IDamagable instead of HPController ??
    {
        if (_troopController == null)
            return;

        Vector3 troopPosition = _troopController.ObjectTransform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        _troopController.StartCoroutine(FightBackCoroutine(enemyHPController, enemyPosition));
    }

    private IEnumerator FightBackCoroutine(HPController enemyHPController, Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(_reactionTime);

        BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
        bulletController.InitializeBullet(_troopController.ObjectTransform.position, enemyPosition);

        yield return new WaitForSeconds(bulletController.GetBulletLifetime());

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyHPController.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {enemyHPController.HPControllerName} with damage {damageUnderAttack}!");
    }
}
