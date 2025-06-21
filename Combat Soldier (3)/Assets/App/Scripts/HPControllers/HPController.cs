using UnityEngine;

public abstract class HPController
{
    protected readonly ScreenCanvasController _troopCanvasController = default;

    protected int _currentHealPoint = default;

    public string _currentName { get; protected set; } = default; // to do 1

    public HPController(ScreenCanvasController troopCanvasController)
    {
        _troopCanvasController = troopCanvasController;
    }

    public void IncreaseHealPoints(int healPoint)
    {
        if (healPoint <= 0)
            return;

        _currentHealPoint += healPoint;

        ChangeSliderAndTextValues();
    }

    public abstract void TakeDamage(int attackDamage);

    public abstract void ActivateDefenseUnderAttack(HPController enemyHPController);

    protected abstract void ChangeSliderAndTextValues();

    protected abstract void CheckHealPointsForBuildingDestroy();

    protected virtual void TroopDeath(MonoBehaviour controller, GameObject objectToDestroy)
    {
        controller.StopAllCoroutines();

        Debug.Log($"The {_currentName} was died");

        Object.DestroyImmediate(objectToDestroy);
    }
}