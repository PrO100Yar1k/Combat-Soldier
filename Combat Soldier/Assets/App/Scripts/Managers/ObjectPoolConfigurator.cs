using UnityEngine;

public class ObjectPoolConfigurator : MonoBehaviour, IInitializableManager
{
    [SerializeField] private Transform _poolParent = default; //
    [SerializeField] private BulletController _bulletPrefab = default; //

    public void InitializeManager()
    {
        SetupPool();
    }

    private void SetupPool()
    {
        if (_bulletPrefab == null)
            return;

        ObjectPooler.SetupPool(_poolParent, _bulletPrefab, 10, "Bullet");
    }
}
