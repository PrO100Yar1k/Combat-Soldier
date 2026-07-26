using UnityEngine;

namespace Assets.App.Scripts.Core.Canvases
{
    public class BuildingWorldCanvasController : MonoBehaviour, IInitializableCanvas<BuildingScriptable>
    {
        public void Initialize(BuildingScriptable data)
        {
            // to do
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
