using UnityEngine;

public abstract class HPController
{
    protected readonly ScreenCanvasController _troopCanvasController = default;

    protected string _currentName = default;

    protected int _currentHealPoint = default;

    public HPController(ScreenCanvasController troopCanvasController)
    {
        _troopCanvasController = troopCanvasController;
    }

    protected abstract void ChangeSliderAndTextValues();

    protected abstract void CheckHealPointsForTroopDeath();

    public abstract void TakeDamage(int attackDamage);

    public void IncreaseHealPoints(int healPoint)
    {
        if (healPoint <= 0)
            return;

        _currentHealPoint += healPoint;

        ChangeSliderAndTextValues();
    }

    protected void TroopDeath(MonoBehaviour controller, GameObject objectToDestroy)
    {
        controller.StopAllCoroutines();

        Debug.Log($"The {_currentName} was died");

        Object.DestroyImmediate(objectToDestroy);
    }
}