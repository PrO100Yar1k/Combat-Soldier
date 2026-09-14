using App.Scripts.Core.Ability;

namespace App.Scripts.Infrastructure.Interfaces
{
    public interface IInitializableCanvas : ICanvasView
    {
        public void Initialize(IStatsController statsController);
    }
}
