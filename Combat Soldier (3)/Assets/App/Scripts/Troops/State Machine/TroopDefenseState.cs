using System;
using UnityEngine;

public class TroopDefenseState : TroopBaseState
{
    private event Action<TroopController> OnActivateDefenseUnderAttack = default;

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

    public TroopDefenseState(TroopController troopController, ISwitchableState switcherState) : base(troopController, switcherState) { }

    public override void Start()
    {
        SubscribeToEvents();
    }

    public override void Stop()
    {
        UnSubscribeFromEvents();
    }

    public void ActivateDefenseUnderAttack(TroopController enemyController)
        => OnActivateDefenseUnderAttack?.Invoke(enemyController);

    private void FightBackToEnemy(TroopController enemyController)
    {
        int damageUnderAttack = _troopScriptable.DamageUnderAttack;
        enemyController.HPController.TakeDamage(damageUnderAttack);

        Debug.Log($"I fought back to {enemyController.TroopScriptable.Name} with damage {damageUnderAttack}");
    }
}
