using System;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    private event Action<HPController> OnActivateDefenseUnderAttack = default;

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

    public void ActivateDefenseUnderAttack(HPController enemyHPController)
        => OnActivateDefenseUnderAttack?.Invoke(enemyHPController);

    protected override void EnableStateIcon()
    {
        Sprite targetIcon = Resources.Load<Sprite>("State Icons/defense_icon");
        _screenCanvasController.ChangeStateIcon(targetIcon);
    }

    private void FightBackToEnemy(HPController enemyHPController)
    {
        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyHPController.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {enemyHPController.HPControllerName} with damage {damageUnderAttack}!");
    }
}
