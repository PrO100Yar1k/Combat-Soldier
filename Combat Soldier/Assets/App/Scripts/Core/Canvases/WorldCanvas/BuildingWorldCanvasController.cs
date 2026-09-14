using UnityEngine;
using App.Scripts.Core.Ability;
using App.Scripts.Infrastructure.Interfaces;

namespace App.Scripts.Core.Canvases.WorldCanvas
{
    public class BuildingWorldCanvasController : MonoBehaviour, IInitializableCanvas
    {
        public void Initialize(IStatsController statsController)
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
