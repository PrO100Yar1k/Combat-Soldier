using System.Collections;
using UnityEngine;

namespace Assets.App.Scripts
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine routine);
    }
}
