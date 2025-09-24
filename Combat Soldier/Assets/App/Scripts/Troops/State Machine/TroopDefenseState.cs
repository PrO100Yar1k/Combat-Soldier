using System;
using System.Collections;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
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

    public TroopDefenseState(TroopController troopController, TroopScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    public void ActivateDefenseUnderAttack(IDamagable enemyIDamagable, Vector3 enemyPosition)
        => OnActivateDefenseUnderAttack?.Invoke(enemyIDamagable, enemyPosition);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(IDamagable enemyIDamagable, Vector3 enemyPosition)
    {
        if (_troopController == null)
            return;

        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        _troopController.StartCoroutine(FightBackCoroutine(enemyIDamagable, enemyPosition));
    }

    private IEnumerator FightBackCoroutine(IDamagable enemyIDamagable, Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(_reactionTime);

        BulletController bulletController = ObjectPooler.DequeueObject<BulletController>("Bullet");
        bulletController.InitializeBullet(_troopController.transform.position, enemyPosition);

        yield return new WaitForSeconds(bulletController.GetBulletLifetime());

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyIDamagable.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {(enemyIDamagable as UnityEngine.Object).name} with damage {damageUnderAttack}!");
    }
}
