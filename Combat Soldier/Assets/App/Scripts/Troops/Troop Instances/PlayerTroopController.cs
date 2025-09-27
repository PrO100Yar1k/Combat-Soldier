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

        Debug.Log("Lord, Your Unit was Damaged!");
    }

    private void ActivateDefenseUnderAttack<T>(T target) where T : MonoBehaviour, IDamagable
    {
        if (StateController.CheckStateForActivity<TroopAttackState>())
            return;

        StateController.ActivateDefenseUnderAttack(target, target.transform.position);
    }
}