using UnityEngine;

namespace App.Scripts.Core.Camera
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _movementSpeed = default;
        [SerializeField] private float _smoothing = default;

        [Header("Zoom Settings")]
        [SerializeField] private float _zoomSpeed = default;
        [SerializeField] private float _minHeight = default;
        [SerializeField] private float _maxHeight = default;

        private Vector3 _targetPosition = default;

        private void Start()
        {
            _targetPosition = transform.position;
        }

        private void Update()
        {
            HandleMouseSwipe();
            HandleZoom();
            SmoothMove();
        }

        private void HandleMouseSwipe()
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                if (Mathf.Abs(mouseX) > 0f || Mathf.Abs(mouseY) > 0f)
                {
                    Vector3 forward = transform.forward;
                    Vector3 right = transform.right;

                    forward.y = 0f;
                    right.y = 0f;

                    forward.Normalize();
                    right.Normalize();

                    Vector3 moveDirection = (right * -mouseX) + (forward * -mouseY);
                    _targetPosition += moveDirection * _movementSpeed;
                }
            }
        }

        private void HandleZoom()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (Mathf.Abs(scrollInput) > 0f)
            {
                Vector3 zoomDirection = transform.forward * (scrollInput * _zoomSpeed);
                Vector3 potentialPosition = _targetPosition + zoomDirection;

                if (potentialPosition.y >= _minHeight && potentialPosition.y <= _maxHeight)
                {
                    _targetPosition = potentialPosition;
                }
                else
                {
                    float clampedY = Mathf.Clamp(potentialPosition.y, _minHeight, _maxHeight);

                    if (!Mathf.Approximately(_targetPosition.y, clampedY))
                        _targetPosition.y = clampedY;
                }
            }
        }

        private void SmoothMove()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smoothing);
        }
    }
}