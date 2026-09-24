using System;
using UnityEngine;

namespace App.Scripts.MVP
{
    public abstract class ViewBase : MonoBehaviour, IView
    {
        public event Action OnVisibilityChanged;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnVisibilityChanged?.Invoke();
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            OnVisibilityChanged?.Invoke();
        }
    }
}
