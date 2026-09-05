namespace App.Scripts.Infrastructure.Interfaces
{
    public interface IInitializableCanvas<TData> : ICanvasView
    {
        public void Initialize(TData data);
    }
}
