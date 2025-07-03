using System;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    private event Action<HPController, Vector3> OnActivateDefenseUnderAttack = default;

    #region Events

    private void SubscribeToEvents()
    {
        OnActivateDefenseUnderAttack += FightBackToEnemy;
    }

    private void UnSubscribeFromEvents()
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

    public void ActivateDefenseUnderAttack(HPController enemyHPController, Vector3 enemyPosition)
        => OnActivateDefenseUnderAttack?.Invoke(enemyHPController, enemyPosition);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(HPController enemyHPController, Vector3 enemyPosition) // IDamagable instead of HPController
    {
        if (_troopController == null)
            return;

        Vector3 troopPosition = _troopController.transform.position;
        float attackRange = _troopController.TroopScriptable.AttackRangeRadius;

        if (Vector3.Distance(troopPosition, enemyPosition) > attackRange)
            return;

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyHPController.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {enemyHPController.HPControllerName} with damage {damageUnderAttack}!");
    }
}
