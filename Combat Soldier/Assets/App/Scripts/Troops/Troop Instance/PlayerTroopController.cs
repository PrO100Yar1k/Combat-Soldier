using UnityEngine;

public class PlayerTroopController : TroopController, IReactableForDamage
{
    public TroopVisionController VisionController { get; private set; }

    protected override void InitializeTroop()
    {
        StateController = new PlayerTroopStateController(this, _screenCanvasController);
        VisionController = new TroopVisionController(this, _troopScriptable);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);
    }

    public void ScreenCanvasUpdateReloadingBar(float timeToReload)
        => (_screenCanvasController as PlayerScreenCanvasController)?.UpdateReloadingBar(timeToReload);

    public bool GetCanvasActivityStateAfterOrder()
        => (_screenCanvasController as PlayerScreenCanvasController).DisableCanvasAfterOrder;


    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        ActivateDefenseUnderAttack(target);

        //ui visible image

        Debug.Log("Lord, Your Unit was Damaged!");
    }

    private void ActivateDefenseUnderAttack<T>(T target) where T : MonoBehaviour, IDamagable
    {
        if (StateController.CheckStateForActivity<TroopAttackState>())
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPos = target.transform.position;

        float attackRange = _troopScriptable.AttackRangeRadius;

        MonoBehaviour enemyInAttackRange = RepositoryManager.instance.GetClosestEnemyInRange(currentPosition, attackRange, TroopSide.Player, null, false);

        if (enemyInAttackRange == null)
            return;
        
        StateController.ActivateDefenseUnderAttack(target, targetPos);
    }
}