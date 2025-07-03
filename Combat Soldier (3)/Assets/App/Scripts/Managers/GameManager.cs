using UnityEngine;

public class GameManager : MonoBehaviour, IInitializeManager
{
    [SerializeField] private BulletController _bulletPrefab = default;

    public void InitializeManager()
        => SetupPool();

    private void SetupPool()
    {
        ObjectPooler.SetupPool(_bulletPrefab, 10, "bullet");
    }
}
