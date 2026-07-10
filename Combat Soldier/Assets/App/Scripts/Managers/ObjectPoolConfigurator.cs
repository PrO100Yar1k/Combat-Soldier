using UnityEngine;

public class ObjectPoolConfigurator : MonoBehaviour
{
    [SerializeField] private Transform _poolParent = default;
    [SerializeField] private BulletController _bulletPrefab = default;

    public void InitializeBulletPool()
    {
        if (_poolParent == null || _bulletPrefab == null)
            return;

        ObjectPooler.SetupPool(_poolParent, _bulletPrefab, 10, "Bullet");
    }
}
