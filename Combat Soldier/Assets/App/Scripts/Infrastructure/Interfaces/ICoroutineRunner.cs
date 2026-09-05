using System.Collections;
using UnityEngine;

namespace App.Scripts.Infrastructure.Interfaces
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine routine);
    }
}
