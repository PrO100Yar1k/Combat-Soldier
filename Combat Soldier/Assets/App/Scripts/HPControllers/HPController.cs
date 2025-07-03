using UnityEngine;

public abstract class HPController
{
    public string HPControllerName { get; protected set; }

    protected int _currentHealPoint = default;

    public void IncreaseHealPoints(int healPoint)
    {
        if (healPoint <= 0)
            return;

        _currentHealPoint += healPoint;

        ChangeSliderAndTextValues();
    }

    public abstract void TakeDamage(int attackDamage);

    protected abstract void AssignBasicParameters<T>(T scriptableObject) where T : ScriptableObject;

    protected abstract void ChangeSliderAndTextValues();

    protected abstract void CheckHealPointsForDeath();

    protected virtual void TroopDeath(MonoBehaviour controller, GameObject objectToDestroy)
    {
        controller.StopAllCoroutines();

        Debug.Log($"The {HPControllerName} was died");

        Object.DestroyImmediate(objectToDestroy);
    }
}