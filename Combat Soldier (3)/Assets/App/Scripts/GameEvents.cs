using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton activation
    [HideInInspector] public static GameEvents instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public event Action OnTroopSpawned = null;
    public void TroopSpawned() => OnTroopSpawned?.Invoke();

    public event Action<Vector3, float> OnTroopMovement = null;
    public void TroopMovement(Vector3 point, float speed) => OnTroopMovement?.Invoke(point, speed);
}
