using System;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    private event Action<TroopController> OnActivateDefenseUnderAttack = default;

    private TroopController AttackingTroop = default;

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

    public TroopDefenseState(TroopController troopController, ScreenCanvasController screenCanvasController, ISwitchableState switcherState) : base(troopController, screenCanvasController, switcherState) { }

    public override void Start()
    {
        EnableStateIcon();
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    public void ActivateDefenseUnderAttack(TroopController enemyController)
        => OnActivateDefenseUnderAttack?.Invoke(enemyController);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(TroopController enemyController)
    {
        if (AttackingTroop == null)
            AttackingTroop = enemyController;

        else if (AttackingTroop != enemyController)
            return;   // do not fight back if troop is already under attack

        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyController.HPController.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {enemyController.TroopScriptable.Name} with damage {damageUnderAttack}!");
    }
}
