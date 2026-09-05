using App.Scripts.Core.Bullet;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.ObjectPool
{
    public class BulletPoolConfigurator : MonoBehaviour, IObjectPool
    {
        [SerializeField] private Transform _poolParent;
        [SerializeField] private BulletController _bulletPrefab;

        public void InitializePool()
        {
            if (_poolParent is null || _bulletPrefab is null)
                return;

            ObjectPooler.SetupPool(_poolParent, _bulletPrefab, 10, "Bullet");
        }
    }
}
