using App.Scripts.Core.ObjectPool;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Core.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private const float _bulletSpeed = 20f;

        private Vector3 _targetPosition = default;

        public void InitializeBullet(Vector3 startPosition, Vector3 targetPosition)
        {
            _targetPosition = targetPosition;

            transform.position = startPosition;
            gameObject.SetActive(true);

            BulletStartMovement();
        }

        public float GetBulletLifetime()
        {
            return Vector3.Distance(transform.position, _targetPosition) / _bulletSpeed;
        }

        private void BulletStartMovement()
        {
            float duration = GetBulletLifetime();

            transform.DOMove(_targetPosition, duration)
                .OnComplete(() => ObjectPooler.EnqueueObject(this, "Bullet"));
        }
    }
}
