using Assets.App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

public class BulletPoolConfigurator : MonoBehaviour, IObjectPool
{
    [SerializeField] private Transform _poolParent = default;
    [SerializeField] private BulletController _bulletPrefab = default;

    public void InitializePool()
    {
        if (_poolParent == null || _bulletPrefab == null)
            return;

        ObjectPooler.SetupPool(_poolParent, _bulletPrefab, 10, "Bullet");
    }
}
