using System.Collections;
using UnityEngine;
using System;

public class EnemyTroopController : TroopController, IReactableForDamage
{
    [SerializeField] protected TroopModelController _troopModelController = default;
    public TroopModelController TroopModelController => _troopModelController;

    protected override void InitializeTroop()
    {
        StateController = new EnemyTroopStateController(this, _screenCanvasController);

        UIController = new UICanvasController<TroopController>(this, _screenCanvasController, _worldCanvasController);
        HPController = new HPControllerTroop(this, _screenCanvasController, _troopScriptable);

        _troopModelController.InitializeModelController(gameObject);

        //StartCoroutine(FindPlayerUnits());
    }

    private IEnumerator FindPlayerUnits()
    {
        while (true)
        {
            const float delay = 1.0f;

            float visibleRange = _troopScriptable.ViewRangeRadius;

            //if ()

            yield return new WaitForSeconds(delay);
        }
    }

    public void ReactionForTakingDamage<T>(T target) where T : MonoBehaviour, IDamagable
    {
        Vector3 targetPos = target.transform.position;
        Action finishAction = () => StateController.ActivateAttackState(target);

        StateController.ActivateMoveState(targetPos, finishAction);
    }
}