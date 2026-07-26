using UnityEngine;

namespace Assets.App.Scripts
{
    public class WorldRangeView : MonoBehaviour
    {
        [SerializeField] private RectTransform _viewCircleRange = default;
        [SerializeField] private RectTransform _attackCircleRange = default;

        public void SetupRanges(float attackRadius, float viewRadius)
        {
            _attackCircleRange.sizeDelta = new Vector2(attackRadius * 2f, attackRadius * 2f);
            _viewCircleRange.sizeDelta = new Vector2(viewRadius * 2f, viewRadius * 2f);
        }

        public void SetCirclesActive(bool isActive)
        {
            _attackCircleRange.gameObject.SetActive(isActive);
            _viewCircleRange.gameObject.SetActive(isActive);
        }
    }
}
