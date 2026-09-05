using App.Scripts.Core.Scriptable;
using App.Scripts.Infrastructure.Interfaces;
using UnityEngine;

namespace App.Scripts.Core.Canvases.WorldCanvas
{
    public class BuildingWorldCanvasController : MonoBehaviour, IInitializableCanvas<BuildingScriptable>
    {
        public void Initialize(BuildingScriptable data)
        {

        }

        public void EnableCanvas()
        {
            gameObject.SetActive(true);
        }

        public void DisableCanvas()
        {
            gameObject.SetActive(false);
        }
    }
}
